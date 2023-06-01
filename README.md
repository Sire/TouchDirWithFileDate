# TouchDirWithFileDate

Console app that will batch change (touch) the modified date on a directory (and optionally all subdirectories recursively) to the same date as the median of the containing files.

This is useful when your OS (like Windows), or some sync / backup app (like Dropbox) has reset the dates on your directories, making sorting by date useless.

Example usage: TouchDirWithFileDate \movies\ --recursive

This is a .NET 7 app and should run fine on [all supported operating systems](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md): Windows, Linux and macOS.
