# Masts Detailed Valuation Request (DVR) Tool

This application is called by the internal VOA VB6 version of Masts in order to generate a PDF containing a Detailed Valuation Request.

The application also supports standalone use for reading fields from PDF files and can be used as a generic PDF field reader and writer.

## Pre-requisites

* Masts VB6 Application - only required if using database features.
* .NET Framework 4.6.1 - this is due to environment settings. The application works fully on .NET 4.7.1
* [Visual Studio Installer project extension](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2017InstallerProjects)

## Build Instructions

Build the Solution in RELEASE mode. There are DEBUG statements that make the application defaulty point to stub files.

## Development Instructions

This program follows a Test Driven Development (TDD) Philosophy. For any changes, please make a corresponding test if possible.

The database for the corresponding sources is required if developing additional database features.

### Adding new datasources

* Create a Struct/Class that inherits from IVOAType within the Types project. The names should replicate the fields on a PDF template.
* Within masts_dvr_tools.DataAccess create a new repository and contract that maps data to the corresponding Type.
* Within the DataManager Service, inject the repository and add it to the Switch Case statement. The case represents the first arguement sent to the .exe

## License

This application is being open-sourced under the AGPL license in order to correspond to [IText 7's community license](https://itextpdf.com/itext7/community).