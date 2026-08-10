namespace IOBluetooth;

/// <summary>
/// IO return values.
/// </summary>
/// <remarks>The values are documented at https://github.com/apple-oss-distributions/xnu/blob/main/iokit/IOKit/IOReturn.h</remarks>
public enum IOReturn
{
    /// <summary>
    /// OK
    /// </summary>
    Success = 0,
    /// <summary>
    /// General error
    /// </summary>
    Error = 0x2bc,
    /// <summary>
    /// Can't allocate memory
    /// </summary>
    NoMemory = 0x2bd,
    /// <summary>
    /// Resource shortage
    /// </summary>
    NoResources = 0x2be,
    /// <summary>
    /// Error during IPC
    /// </summary>
    IpcError = 0x2bf,
    /// <summary>
    /// No such devic
    /// </summary>
    NoDevice = 0x2c0,
    /// <summary>
    /// Privilege violation
    /// </summary>
    NotPrivileged = 0x2c1,
    /// <summary>
    /// Invalid argument
    /// </summary>
    BadArgument = 0x2c2,
    /// <summary>
    /// Device read locked
    /// </summary>
    LockedRead = 0x2c3,
    /// <summary>
    /// Device write locked
    /// </summary>
    LockedWrite = 0x2c4,
    /// <summary>
    /// Exclusive access and device already open
    /// </summary>
    ExclusiveAccess = 0x2c5,
    /// <summary>
    /// Sent/received messages had different msg_id
    /// </summary>
    BadMessageId = 0x2c6,
    /// <summary>
    /// Unsupported function
    /// </summary>
    Unsupported = 0x2c7,
    /// <summary>
    /// Misc. VM failure
    /// </summary>
    VmError = 0x2c8,
    /// <summary>
    /// Internal error
    /// </summary>
    InternalError = 0x2c9,
    /// <summary>
    /// General I/O error
    /// </summary>
    IOError = 0x2ca,
    /// <summary>
    /// Can't acquire lock
    /// </summary>
    CannotLock = 0x2cc,
    /// <summary>
    /// Device is not open
    /// </summary>
    NotOpen = 0x2cd,
    /// <summary>
    /// Read not supported
    /// </summary>
    NotReadable = 0x2ce,
    /// <summary>
    /// Write not supported
    /// </summary>
    NotWritable = 0x2cf,
    /// <summary>
    /// Alignment error
    /// </summary>
    NotAligned = 0x2d0,
    /// <summary>
    /// Media error
    /// </summary>
    BadMedia = 0x2d1,
    /// <summary>
    /// Device(s) still open
    /// </summary>
    StillOpen = 0x2d2,
    /// <summary>
    /// Rld failure
    /// </summary>
    RldError = 0x2d3,
    /// <summary>
    /// DMA failure
    /// </summary>
    DmaError = 0x2d4,
    /// <summary>
    /// Device is busy
    /// </summary>
    Busy = 0x2d5,
    /// <summary>
    /// I/O Timeout
    /// </summary>
    Timeout = 0x2d6,
    /// <summary>
    /// Device offline
    /// </summary>
    Offline =  0x2d7,
    /// <summary>
    /// Not ready
    /// </summary>
    NotReady = 0x2d8,
    /// <summary>
    /// Device is not attached
    /// </summary>
    NotAttached =  0x2d9,
    /// <summary>
    /// No DMA channels left
    /// </summary>
    NoChannels = 0x2da,
    /// <summary>
    /// No space for data
    /// </summary>
    NoSpace = 0x2db,
    /// <summary>
    /// Port already exists
    /// </summary>
    PortExists = 0x2dd,
    /// <summary>
    /// Can't wire down physical memory
    /// </summary>
    CannotWire = 0x2de,
    /// <summary>
    /// No interrupt attached
    /// </summary>
    NoInterrupt =  0x2df,
    /// <summary>
    /// No DMA frames enqueued
    /// </summary>
    NoFrames = 0x2e0,
    /// <summary>
    /// Oversized msg received on interrupt port
    /// </summary>
    MessageTooLarge = 0x2e1,
    /// <summary>
    /// Not permitted
    /// </summary>
    NotPermitted =  0x2e2,
    /// <summary>
    /// No power to the device
    /// </summary>
    NoPower = 0x2e3,
    /// <summary>
    /// Media is not present
    /// </summary>
    NoMedia = 0x2e4,
    /// <summary>
    /// Media is not formatted
    /// </summary>
    UnformattedMedia = 0x2e5,
    /// <summary>
    /// No such mode
    /// </summary>
    UnsupportedMode = 0x2e6,
    /// <summary>
    /// Data underrun
    /// </summary>
    Underrun = 0x2e7,
    /// <summary>
    /// Data overrun
    /// </summary>
    Overrun = 0x2e8,
    /// <summary>
    /// The device is not working properly!
    /// </summary>
    DeviceError = 0x2e9,
    /// <summary>
    /// A completion routine is required
    /// </summary>
    NoCompletion = 0x2ea,
    /// <summary>
    /// Operation aborted
    /// </summary>
    Aborted = 0x2eb,
    /// <summary>
    /// Bus bandwidth would be exceeded
    /// </summary>
    NoBandwidth = 0x2ec,
    /// <summary>
    /// Device not responding
    /// </summary>
    NotResponding = 0x2ed,
    /// <summary>
    /// Isochronous I/O request for distant past!
    /// </summary>
    IsoTooOld = 0x2ee,
    /// <summary>
    /// Isochronous I/O request for distant future
    /// </summary>
    IsoTooNew = 0x2ef,
    /// <summary>
    /// Data was not found
    /// </summary>
    NotFound = 0x2f0,
    /// <summary>
    /// Should never be seen
    /// </summary>
    Invalid = 0x1,
}