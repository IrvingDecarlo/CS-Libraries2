Cephei C# (.Net) Libraries Version 2,
created and maintained by Irving Decarlo.

# What is CS-Libraries2?

The original CS-Libraries project was initially created for my own personal use back in 2019. However, I was an inexperienced C# developer back then, so the original library fell short from my expectations.

Hence, in late 2020, I scrapped the original library and rewrote everything from scratch, thus dawning this current new version, the CS-Libraries2 project.

CS-Libraries2 is a general-purpose collection of high-level .Net libraries. All of the libraries in the project are written and compiled for .Net Standard 2.1.

## Remarks

CS-Libraries2 was made for my own personal use and therefore will be modified according to my own needs.

It is worth noting that retroactive compatibility is not guaranteed - in fact, modifications made in any of the libraries may be incompatible with code created for former versions, whether fully or partially.

To better understand when a new version is compatible or not, refer to the following section: Versioning.

## Versioning

Versioning in CS-Libraries2 emphasises more on how compatible the new version is compared to its predecessor rather than in the amount of changes made. A new version may contain multiple changes while being fully compatible, whereas others with a much lesser changelog may not be fully functional with code written for a previous version.

All libraries follow the standard 4-number version format (x.x.x.x).

In order to keep track of how compatible the new version is, the libraries under CS-Libraries2 are in accordance to the following rule:

### Major Versions (X.x.x.x): Library Rewritten from Scratch

When a library is updated to a new Major version, it means that it was rewritten from scratch. Therefore, the new version is to be considered fully incompatible with any previous versions.

### Minor Versions (x.X.x.x): Update with Partial Incompatibilities

When a library is updated to a new Minor version, it means that the update is expected to be partially incompatible with a previous version. Work may be required to adjust existing code to the new version.

### Release Versions (x.x.X.x): Update with No Incompatibilities

When a library is updated to a new Release version, it means that the update is expected to be fully compatible with a previous version. No changes in already existing code will have to be made. Release versions often include new functionalities.

### Build Versions (x.x.x.X): Patches and Bugfixes

When a library is updated to a new Build version, it means that patches and bugfixes were implemented which do not break compatibility with the previous version. It does not often include new functionalities.

## How can I Contribute?

Even though I much appreciate the willingness to contribute to this repository, it was made for my personal use. Therefore, I need it to remain accessible to me at all times without modifications.

If there are changes that you'd like implemented or if you have found bugs, feel free to open a new issue in the repository and I will have a look at it.

You may also clone this repository to one of your own and make the modifications you'd like. If that's the case, please do modify the Project files of the library you'll modify and replace my name and Cephei with your own. I will not take responsibility for libraries based on my own that do not belong to this repository.

Thank you!
