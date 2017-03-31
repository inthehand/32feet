using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;
using System.IO;
using Microsoft.TeamFoundation;

namespace ConsoleMenuTesting
{
    partial class BluetoothTesting
    {

        [MenuItem]
        public void TfsProjectDiff()
        {
            TfsProjectDiff(new string[0]);
        }

        void TfsProjectDiff(string[] args)
        {
            // Figure out the workspace information based on the local cache. 
            WorkspaceInfo wsInfo = Workstation.Current.GetLocalWorkspaceInfo(Environment.CurrentDirectory);
            if (wsInfo == null) {
                Console.Error.WriteLine("The current directory is not mapped.");
                Environment.Exit(1);
            }

            // Now we can get to the workspace. 
            TeamFoundationServer tfs = new TeamFoundationServer(wsInfo.ServerUri.AbsoluteUri,
                new System.Net.NetworkCredential(@"snd\alanjmcf_cp", "acpsbjorg"));
            tfs.Authenticate();
            Workspace workspace = wsInfo.GetWorkspace(tfs);

            try {
                // Display the differences for the current directory and all of its descendants 
                // (fully recursive). 
                if (args.Length == 0) {
                    // Display the differences between what's in the workspace compared to latest. 
                    DisplayCurrentDiff(workspace);
                } else {
                    // Display the differences between the two specified versions. 
                    if (args.Length != 2) {
                        Console.Error.WriteLine("Usage: projectdiff <versionspec> <versionspec>");
                        Console.Error.WriteLine("Example: projectdiff D04/06/06 T");
                        Console.Error.WriteLine("         compare midnight April 6, 2006 and latest");
                        Console.Error.WriteLine("Example: projectdiff W T");
                        Console.Error.WriteLine("         compare what's in the workspace to latest");
                        Environment.Exit(1);
                    }

                    // Parse the  
                    VersionSpec version1 = ParseVersionSpec(args[0], workspace);
                    VersionSpec version2 = ParseVersionSpec(args[1], workspace);

                    DisplayVersionDiff(workspace, version1, version2);
                }
            } catch (TeamFoundationServerException e) {
                // If something goes wrong, such as not having access to the server, display 
                // the appropriate error message. 
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        // This approach uses the same call that Source Control Explorer uses. 
        static void DisplayCurrentDiff(Workspace workspace)
        {
            // Let's get a list of the files and folders from the server.  The ExtendedItem 
            // contains information about which version is in the workspace and the latest version 
            // on the server. 
            // You would want to use this approach if you wanted to display the type of 
            // information available in the Source Control Explorer. 
            ItemSpec[] querySpec = new ItemSpec[] { new ItemSpec(Environment.CurrentDirectory, 
                                                                 RecursionType.Full) };
            ExtendedItem[] items = workspace.GetExtendedItems(querySpec, DeletedState.NonDeleted,
                                                              ItemType.Any)[0];

            // Now let's display what we know about each item. 
            DisplayHeader("Files and folders that are at the latest versions:");
            foreach (ExtendedItem item in items) {
                if (item.IsLatest) {
                    DisplayExtendedItem(item, workspace);
                }
            }

            DisplayHeader("Files and folders that are out of date:");
            foreach (ExtendedItem item in items) {
                if (item.IsInWorkspace && !item.IsLatest) {
                    DisplayExtendedItem(item, workspace);
                }
            }

            DisplayHeader("Files and folders not in the workspace:");
            foreach (ExtendedItem item in items) {
                if (!item.IsInWorkspace) {
                    DisplayExtendedItem(item, workspace);
                }
            }
            Console.WriteLine();
        }

        // Displays change and path information from the ExtendedItem. 
        static void DisplayExtendedItem(ExtendedItem item, Workspace workspace)
        {
            Console.Write("  ");

            // Indicate whether someone else has a pending change on this file or folder.  The 
            // ExtendedItem doesn't contain the list of users with pending changes on this file. 
            // For that information, we'd need to call QueryPendingSets() in addition to  
            // GetExtendedItems() and join the two together via the item ID. 
            if (item.HasOtherPendingChange) {
                Console.Write("^ ");
            }

            // Show the lock information if someone has locked the file or folder. 
            if (item.LockStatus != LockLevel.None) {
                Console.Write("[{0},{1}] ",
                                    PendingChange.GetLocalizedStringForLockLevel(item.LockStatus),
                                    item.LockOwner);
            }

            // If there is a change pending on the item in the current workspace, display it. 
            if (item.ChangeType != ChangeType.None) {
                Console.Write("({0}) ", PendingChange.GetLocalizedStringForChangeType(item.ChangeType));
            }

            // Display the local path. 
            if (!item.IsInWorkspace) {
                // Get the mapping so that we can determine its local path or that it is cloaked. 
                WorkingFolder wf = workspace.TryGetWorkingFolderForServerItem(item.TargetServerItem);

                // Let's skip cloaked items, since the user doesn't want them. 
                if (!wf.IsCloaked) {
                    Console.WriteLine(wf.LocalItem);
                }
            } else {
                Console.WriteLine(item.LocalItem);
            }
        }

        // This approach compares two different versions of the tree.  When version1 is W 
        // (the workspace version spec) and version2 is T (tip/latest), the results will be 
        // be equivalent to the other approach (we get some different properties). 
        static void DisplayVersionDiff(Workspace workspace, VersionSpec version1,
                                       VersionSpec version2)
        {
            // We need the list of items at the specified versions.  This call only gets 
            // information about the versions that have been checked in and does not include 
            // any pending changes.  As a result, it does not return pending adds, branches, 
            // and undeletes. 
            ItemSet itemSet1 = workspace.VersionControlServer.GetItems(Environment.CurrentDirectory,
                                                                       version1, RecursionType.Full);
            ItemSet itemSet2 = workspace.VersionControlServer.GetItems(Environment.CurrentDirectory,
                                                                       version2, RecursionType.Full);
            Item[] items1 = itemSet1.Items;
            Item[] items2 = itemSet2.Items;

            // Build hash tables of the items so that we can quickly determine which items 
            // are common.  Every item in the repository is assigned a unique item ID. 
            // The item ID never changes, even though the item's path (item.ServerItem) may 
            // change due to being renamed or moved. 
            Dictionary<int, Item> itemHash1 = CreateHash(items1);
            Dictionary<int, Item> itemHash2 = CreateHash(items2);

            // Show items that are the same. 
            DisplayHeader("Same:");
            foreach (Item item in itemHash1.Values) {
                Item item2;
                if (itemHash2.TryGetValue(item.ItemId, out item2) &&
                    item2.ChangesetId == item.ChangesetId) {
                    Console.WriteLine("  " + item2.ServerItem);
                }
            }

            // Show items that differ. 
            DisplayHeader("Different:");
            foreach (Item item in itemHash1.Values) {
                Item item2;
                if (itemHash2.TryGetValue(item.ItemId, out item2) &&
                    item2.ChangesetId != item.ChangesetId) {
                    // Figure out what changed. 
                    bool showDiff = false;
                    if (item.ItemType == ItemType.File &&
                        !EqualFileContents(item, item2)) {
                        Console.Write('e');
                        showDiff = true;
                    } else if (item.Encoding != item2.Encoding) {
                        Console.Write('n');
                    } else if (item.DeletionId != item2.DeletionId) {
                        Console.Write('d');
                    } else if (item.ServerItem != item2.ServerItem) {
                        // Note that we used a case-sensitive comparison in order to catch 
                        // renames where only the case changed. 
                        Console.Write('r');
                    }

                    Console.WriteLine(" {0}", item2.ServerItem);

                    if (showDiff) {
                        DiffFiles(item, item2);
                    }
                }
            }

            // Show items only in the first version. 
            DisplayHeader("Only in " + version1.DisplayString + ":");
            DisplayOnlyInFirst(itemHash1, itemHash2);

            // Show items only in the second version. 
            DisplayHeader("Only in " + version2.DisplayString + ":");
            DisplayOnlyInFirst(itemHash2, itemHash1);
        }

        // Fill in the workspace name if it is null. 
        static VersionSpec ParseVersionSpec(String spec, Workspace workspace)
        {
            String user = workspace.VersionControlServer.TeamFoundationServer.AuthenticatedUserName;

            VersionSpec version = VersionSpec.ParseSingleSpec(spec, user);

            // If the user happened to specify only W for the workspace spec, we'll have to 
            // fill in the workspace here (the parse method doesn't know it). 
            WorkspaceVersionSpec wvs = version as WorkspaceVersionSpec;
            if (wvs != null && wvs.Name == null) {
                wvs.Name = workspace.Name;
            }

            return version;
        }

        // Creates a hash table of the files and folders. 
        static Dictionary<int, Item> CreateHash(Item[] items)
        {
            Dictionary<int, Item> itemHash = new Dictionary<int, Item>();
            foreach (Item item in items) {
                itemHash.Add(item.ItemId, item);
            }

            return itemHash;
        }

        // Displays files and folders that are only in the first hash table. 
        static void DisplayOnlyInFirst(Dictionary<int, Item> itemHash1,
                                       Dictionary<int, Item> itemHash2)
        {
            foreach (Item item in itemHash1.Values) {
                if (!itemHash2.ContainsKey(item.ItemId)) {
                    Console.WriteLine("  " + item.ServerItem);
                }
            }
        }

        // Returns true if the contents of the two versions of the file are the same. 
        static bool EqualFileContents(Item item1, Item item2)
        {
            if (item1.ContentLength != item2.ContentLength) {
                return false;
            }

            // If the two hash values have different lengths or both have a length of zero, 
            // the files are not the same.  The only time this would happen would be for 
            // files uploaded by clients that have FIPS enforcement enabled (rare). 
            // Those clients can't compute the MD5 hash, so it has a length of zero in that 
            // case.  To do this right with FIPS, the code would need to compare file 
            // contents (call item.DownloadFile()). 
            // For information on FIPS enforcement and MD5, see the following link. 
            // http://blogs.msdn.com/shawnfa/archive/2005/05/16/417975.aspx 
            if (item1.HashValue.Length != item2.HashValue.Length ||
                item1.HashValue.Length == 0) {
                return false;
            }

            for (int i = 0; i < item1.HashValue.Length; i++) {
                if (item1.HashValue[i] != item2.HashValue[i]) {
                    return false;
                }
            }

            return true;
        }

        // Display the differences between the two file versions. 
        static void DiffFiles(Item item1, Item item2)
        {
            if (item1.ItemType != ItemType.File) {
                return;
            }

            Console.WriteLine();

            DiffItemVersionedFile diffItem1 = new DiffItemVersionedFile(item1,
                                                       new ChangesetVersionSpec(item1.ChangesetId));
            DiffItemVersionedFile diffItem2 = new DiffItemVersionedFile(item2,
                                                       new ChangesetVersionSpec(item2.ChangesetId));

            // Here we set up the options to show the diffs in the console with the unified diff 
            // format. 
            // If you simply want to launch the external diff viewer, rather than get a text diff, 
            // you just need to set UseThirdPartyTool to true.  You don't need to set any of the 
            // other properties to use the external tool. 
            DiffOptions options = new DiffOptions();
            options.UseThirdPartyTool = false;

            // These settings are just for the text diff (not needed for an external tool). 
            options.Flags = DiffOptionFlags.EnablePreambleHandling | DiffOptionFlags.IgnoreWhiteSpace;
            options.OutputType = DiffOutputType.Unified;
            options.TargetEncoding = Console.OutputEncoding;
            options.SourceEncoding = Console.OutputEncoding;
            options.StreamWriter = new StreamWriter(Console.OpenStandardOutput(),
                                                    Console.OutputEncoding);
            options.StreamWriter.AutoFlush = true;

            // The last parameter indicates whether the code should block until the external diff 
            // viewer exits.  Set it to false if you are calling this from a GUI app. 
            Difference.DiffFiles(item1.VersionControlServer, diffItem1, diffItem2,
                                 options, item1.ServerItem, true);
        }

        static void DisplayHeader(String header)
        {
            Console.WriteLine();
            Console.WriteLine(String.Empty.PadLeft(80, '='));
            Console.WriteLine(header);
        }
    }
}
