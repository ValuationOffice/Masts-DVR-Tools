# Masts Detailed Valuation Request (DVR) Tool

This application is called by the internal VOA VB6 version of Masts in order to generate a PDF containing a Detailed Valuation Request.

The application also supports standalone use for reading fields from PDF files and can be used as a generic PDF field reader and writer.

## Pre-requisites

* Masts VB6 Application - only required if using database features.
* .NET Framework 4.7.1

## Build Instructions

Build the Solution in RELEASE mode. There are DEBUG statements that make the application defaulty point to stub files.

## Development Instructions

This program follows a Test Driven Development (TDD) Philosophy. For any changes, please make a corresponding test if possible.

The database for the corresponding sources is required if developing additional database features.