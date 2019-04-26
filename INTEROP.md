# dnYara

## Interoperability

**dnYara** is a `.Net Standard 2.0` wrapper for the native Yara library. It will lets you use all the features of the native C Yara library in C# for a large set of .Net framework and plateforme.

Indeed, the fact it is built for `.Net Standard 2.0` ensure interoperability and portability with a maximum of .Net frameworks, and os-platform ([learn more](INTEROP.md)).

Existing Yara wrappers for .Net:
- are developped in C++/CLR .Net because it allows to use native .lib libraries
- are only compatible with .Net Framework (and therefore only with Windows OS)

So I decided to write it in C# under .Net Standard, via PInvoke API mapping. 

As a reminder, the motivation behind .Net Standard is to establish greater uniformity in the .Net eco-system. `ECMA 335` continues to establish uniformity for .Net implementation behavior, but there is no similar specification for.NET base class libraries (BCLs) for .Net implementations.

The .Net Standard allows the following key scenarios:

- Defines a uniform set of BCL APIs for all .NET implementations to be implemented, regardless of workload.
- Helps developers produce portable libraries that can be used on all .NET implementations, using the same set of APIs.
Reduces or even eliminates conditional compilation of the shared source due to API.NET, only for OS APIs.

![.Net Standard Img](https://i.stack.imgur.com/tE1ny.png)

In concrete terms, the current version of dnYara can be used on the following frameworks:

|.NET Standard|  [2.0] |
|:-------------------------------------|-------------:|
|.NET Core                             | **2.0**      |
|.NET Framework                        | **4.6.1**    |
|Mono                                  | **5.4**      |
|Xamarin.iOS                           |**10.14**     |
|Xamarin.Mac                           | **3.8**      |
|Xamarin.Android                       | **8.0**      |
|Unity                                 |**2018.1**    |
|Universal Windows Platform            |**10.0.16299**|

### .Net Core 2.0 - Supported OS versions
.NET Core is an open-source, general-purpose development platform maintained by Microsoft and the .NET community on GitHub. 
It's cross-platform (supporting Windows, macOS, and Linux) and can be used to build device, cloud, and IoT applications.

#### Windows

OS                            | Version                       | Architectures  | Notes
------------------------------|-------------------------------|----------------|-----
Windows Client                | 7 SP1+, 8.1                  | x64, x86       |
Windows 10 Client             | Version 1607+                 | x64, x86       |
Windows Server                | 2008 R2 SP1+                  | x64, x86       |

See the [Windows Lifecycle Fact Sheet](https://support.microsoft.com/en-us/help/13853/windows-lifecycle-fact-sheet) for details regarding each Windows release lifecycle.

#### macOS

OS                            | Version                       | Architectures  | Notes
------------------------------|-------------------------------|----------------|-----
Mac OS X                      | 10.12+                        | x64            | [Apple Support Sitemap](https://support.apple.com/sitemap) <br> [Apple Security Updates](https://support.apple.com/en-us/HT201222)



#### Linux

OS                            | Version                       | Architectures  | Notes
------------------------------|-------------------------------|----------------|-----
Red Hat Enterprise Linux      | 6                             | x64            | [Microsoft support policy](https://www.microsoft.com/net/support/policy) 
Red Hat Enterprise Linux <br> CentOS <br> Oracle Linux     | 7                             | x64            | [Red Hat support policy](https://access.redhat.com/support/policy/updates/errata/) <br> [CentOS lifecycle](https://wiki.centos.org/FAQ/General#head-fe8a0be91ee3e7dea812e8694491e1dde5b75e6d) <br> [Oracle Linux lifecycle](http://www.oracle.com/us/support/library/elsp-lifetime-069338.pdf)
Fedora                        | 27                | x64            | [Fedora lifecycle](https://fedoraproject.org/wiki/End_of_life)
Debian                        | 9, 8.7+                   | x64            | [Debian lifecycle](https://wiki.debian.org/DebianReleases)
Ubuntu                        | 18.04, 16.04, 14.04       | x64            | [Ubuntu lifecycle](https://wiki.ubuntu.com/Releases)
Linux Mint                    | 18, 17                    | x64            | [Linux Mint end of life announcements](https://forums.linuxmint.com/search.php?keywords=%22end+of+life%22&terms=all&author=&sc=1&sf=titleonly&sr=posts&sk=t&sd=d&st=0&ch=300&t=0&submit=Search)
openSUSE                      | 42.3+                         | x64            | [OpenSUSE lifecycle](https://en.opensuse.org/Lifetime)
SUSE Enterprise Linux (SLES)  | 12 SP2+                   | x64            | [SUSE lifecycle](https://www.suse.com/lifecycle/)

* **Bold numbers** indicate additions in this release.
* '+' indicates the minimum supported version.
