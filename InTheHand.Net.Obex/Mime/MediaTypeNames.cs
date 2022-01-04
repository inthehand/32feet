// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Mime.MediaTypeNames
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Mime
{
	/// <summary>
	/// Specifies the media type information for an object.
	/// </summary>
	/// <seealso cref="System.Net.Mime.MediaTypeNames"/>
	public static class MediaTypeNames
	{

		/// <summary>
		/// Specifies the type of text data in an object.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
		public static class Text
		{
			/// <summary>
			/// Specifies that the data is in vCalendar format.
			/// </summary>
#if CODE_ANALYSIS
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
#endif
			public const string vCalendar = "text/x-vcal";

			/// <summary>
			/// Specifies that the data is in vCard format.
			/// </summary>
#if CODE_ANALYSIS
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
#endif
			public const string vCard = "text/x-vcard";

			/// <summary>
			/// Specifies that the data is in vMsg format.
			/// </summary>
#if CODE_ANALYSIS
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
#endif
			public const string vMessage = "text/x-vMsg";

			/// <summary>
			/// Specifies that the data is in vNote format.
			/// </summary>
#if CODE_ANALYSIS
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
#endif
			public const string vNote = "text/x-vnote";

			/// <summary>
			/// Specifies that the data is in vToDo format.
			/// </summary>
#if CODE_ANALYSIS
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
#endif
			public const string vToDo = "text/x-vtodo";

			
		}

		/// <summary>
		/// Specifies the type of Object Exchange specific data.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public static class ObjectExchange
		{
			/// <summary>
			/// Used to retrieve supported object types.
			/// </summary>
			public const string Capabilities = "x-obex/capability";

			/// <summary>
			/// Used to retrieve folder listing with OBEX FTP.
			/// </summary>
			public const string FolderListing = "x-obex/folder-listing";

			/// <summary>
			/// Used to retrieve an object profile.
			/// </summary>
			public const string ObjectProfile = "x-obex/object-profile";
		}

		/// <summary>
		/// Specifies the type of Messaging Access Profile (MAP) specific data.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public static class Messaging
		{
			/// <summary>
			/// Used to retrieve a text message.
			/// </summary>
			public const string Message = "x-bt/message";

			/// <summary>
			/// Used to retrieve message listing with MAP.
			/// </summary>
			public const string MessageListing = "x-bt/MAP-msg-listing";

			/// <summary>
			/// Used to report events.
			/// </summary>
			public const string EventReport = "x-bt/MAP-event-report";
		}

		/// <summary>
		/// Specifies the type of Phone Book Access Profile (PBAP) specific data.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public static class PhoneBookAccess
		{
			/// <summary>
			/// Used to retrieve a phone book. This is a collection of vCard items with custom fields.
			/// </summary>
			public const string PhoneBook = "x-bt/phonebook";

			/// <summary>
			/// Used to retrieve an individual vCard entry.
			/// </summary>
			public const string VCard = "x-bt/vcard";

			/// <summary>
			/// Used to retrieve a vCard listing from a query.
			/// </summary>
			public const string VCardListing = "x-bt/vcard-listing";
		}
	}
}
