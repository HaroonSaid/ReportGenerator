# ReportGenerator
ReportGenerator converts XML reports generated by OpenCover, PartCover, dotCover, Visual Studio, NCover or Cobertura into human readable reports in various formats.

The reports do not only show the coverage quota, but also include the source code and visualize which lines have been covered.

ReportGenerator supports merging several reports into one.
It is also possible to pass one XML file containing several reports to ReportGenerator (e.g. a build log file).

The following output formats are supported by ReportGenerator:
* HTML, HTMLSummary, HTMLChart, [MHTML](https://en.wikipedia.org/wiki/MHTML)
* XML, XMLSummary
* Latex, LatexSummary
* TextSummary
* CsvSummary
* PngChart
* Badges
* [Custom reports](https://github.com/danielpalme/ReportGenerator/wiki/Custom-reports)

**Compatibility:**
* [OpenCover](https://github.com/OpenCover/opencover) ([Nuget](https://www.nuget.org/packages/OpenCover))
* [PartCover](https://github.com/sawilde/partcover.net4) 4.0 ([Nuget](https://www.nuget.org/packages/partcovernet4/))
* [PartCover](http://sourceforge.net/projects/partcover/) 2.2, 2.3
* [dotCover](https://www.jetbrains.com/dotcover/help/dotCover__Console_Runner_Commands.html) ([Nuget](https://www.nuget.org/packages/JetBrains.dotCover.CommandLineTools/), /ReportType=DetailedXML)
* Visual Studio ([vstest.console.exe](https://github.com/danielpalme/ReportGenerator/wiki/Visual-Studio-Coverage-Tools#vstestconsoleexe), [CodeCoverage.exe](https://github.com/danielpalme/ReportGenerator/wiki/Visual-Studio-Coverage-Tools#codecoverageexe))
* [NCover](http://www.ncover.com/download/current) (tested version 1.5.8, other versions may not work)
* [Cobertura](https://github.com/cobertura/cobertura)
* Mono ([mprof-report](http://www.mono-project.com/docs/debug+profile/profile/profiler/#analyzing-the-profile-data)) 

Also available as **NuGet** package: http://www.nuget.org/packages/ReportGenerator

Additional information about ReportGenerator can be found under [Resources](#resources).

Author: Daniel Palme  
Blog: [www.palmmedia.de](http://www.palmmedia.de)  
Twitter: [@danielpalme](http://twitter.com/danielpalme)  

## Screenshots
The screenshots show two snippets of the generated reports:
![Screenshot 1](http://danielpalme.github.io/ReportGenerator/resources/screenshot1.png)
![Screenshot 2](http://danielpalme.github.io/ReportGenerator/resources/screenshot2.png)

### Badges
Badges in SVG format can be generated if `-reporttypes:Badges` is used:

![Sample badge](http://danielpalme.github.io/ReportGenerator/resources/badge.svg)

## Usage
ReportGenerator is a commandline tool which requires the following parameters:

```
Parameters:
    ["]-reports:<report>[;<report>][;<report>]["]
    ["]-targetdir:<target directory>["]
    [["]-reporttypes:<Html|HtmlSummary|...>[;<Html|HtmlSummary|...>]["]]
    [["]-sourcedirs:<directory>[;<directory>][;<directory>]["]]
    [["]-historydir:<history directory>["]]
    [["]-assemblyfilters:<(+|-)filter>[;<(+|-)filter>][;<(+|-)filter>]["]]
    [["]-classfilters:<(+|-)filter>[;<(+|-)filter>][;<(+|-)filter>]["]]
    [["]-verbosity:<Verbose|Info|Error>["]]

Explanations:
   Reports:           The coverage reports that should be parsed (separated by 
                      semicolon). Wildcards are allowed.
   Targetdirectory:   The directory where the generated report should be saved.
   Reporttypes:       The output formats and scope (separated by semicolon).
                      Values: Badges, Html, HtmlSummary, Latex, LatexSummary, TextSummary, Xml, XmlSummary
   SourceDirectories: Optional directories which contain the corresponding source code
                      (separated by semicolon).
                      The source files are used if coverage report contains classes
                      without path information.
   History directory: Optional directory for storing persistent coverage information.
                      Can be used in future reports to show coverage evolution.
   Assembly Filters:  Optional list of assemblies that should be included or excluded in the report.
   Class Filters:     Optional list of classes that should be included or excluded in the report.
                      Exclusion filters take precedence over inclusion filters.                      
                      Wildcards are allowed.
   Verbosity:         The verbosity level of the log messages.
                      Values: Verbose, Info, Error

Default values:
   -reporttypes:Html
   -assemblyfilters:+*
   -classfilters:+*
   -verbosity:Verbose

Examples:
   "-reports:coverage.xml" "-targetdir:C:\report"
   "-reports:target\*\*.xml" "-targetdir:C:\report" -reporttypes:Latex;HtmlSummary
   "-reports:coverage1.xml;coverage2.xml" "-targetdir:report"
   "-reports:coverage.xml" "-targetdir:C:\report" -reporttypes:Latex "-sourcedirs:C:\MyProject"
   "-reports:coverage.xml" "-targetdir:C:\report" "-sourcedirs:C:\MyProject1;C:\MyProject2" "-assemblyfilters:+Included;-Exclude
d.*"
```

A MSBuild task also exists:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project DefaultTargets="Coverage" xmlns="http://schemas.microsoft.com/developer/msbuild/2003" ToolsVersion="4.0">
  <UsingTask TaskName="ReportGenerator" AssemblyFile="ReportGenerator.exe" />
  <ItemGroup>
    <CoverageFiles Include="partcover.xml" />
    <SourceDirectories Include="C:\MyProject1" />
    <SourceDirectories Include="C:\MyProject2" />
  </ItemGroup>
  <Target Name="Coverage">
    <ReportGenerator ReportFiles="@(CoverageFiles)" TargetDirectory="report" ReportTypes="Html;Latex" SourceDirectories="@(SourceDirectories)" HistoryDirectory="history" AssemblyFilters="+Include;-Excluded" VerbosityLevel="Verbose" />
  </Target>
</Project>
```

## Resources

* http://www.palmmedia.de/Blog/2016/11/6/reportgenerator-new-release-with-enhanced-html-report-and-cobertura-support
* http://www.palmmedia.de/Blog/2015/1/27/reportgenerator-new-beta-with-historytrend-charts
* http://www.palmmedia.de/Blog/2012/4/29/reportgenerator-new-release-with-more-advanced-report-preprocessing
* http://www.palmmedia.de/Blog/2009/10/30/msbuild-code-coverage-analysis-with-partcover-and-reportgenerator
* http://www.palmmedia.de/Blog/2010/2/15/partcover-coverage-of-unexecuted-methods
* http://www.palmmedia.de/Blog/2010/5/2/partcover-coverage-of-unexecuted-methods-part-2
* http://www.mellekoning.nl/index.php/2010/02/13/unit-testing-coverage-with-partcover-and-reportgenerator
