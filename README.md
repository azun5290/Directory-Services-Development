initial test

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

Adding Directory Services C# project files

Historical notes

The AD Directory Services C# consolle was started during a large infrastructure project I was involved with at the end of 2012, where there was a sudden need for the automation of a large number of Active Directory tasks, such as bulk addition of hundreds of subnets and sites to AD sites and services, and also potentially, the subsequent bulk modification and/or deletion of the objects themselves from AD, and/or modification and/or deletion of some of their attributes. The console proved quite handy also for reporting, which is incredibly cumbersome in AD Sites and Services (both in the W2K3 and W2K8 server console versions).

This had to be accomplished very quickly. At that time, unlike now, there were no PowerShell scripts for this set of tasks readily available online. VB was too cumbersome for AD sites and services. My response was a three week foray into the world of MS DS (Directory Services) System.DirectoryServices.ActiveDirectory Namespace. As a result, I produced an AD console tool which achieved this in a very robust manner for my purposes at the time. However, many functions are still quite buggy and like anything else, this can be massively improved and taken to brand new levels, especially becasue further infrastructure work caused the abrupt interruption of the AD shell development. It's kind of like a dead mining town. Started out, but abandoned now.

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

I have limited time so I'll add some notes I jotted down at the time. Forgive the roughness. A quick note: I am not a software developer, so my code is potentially quite raw in certain areas of the namespace/s, but as I am essentially an infrastructure/security engineer, it quite relentlessly always "does the job".

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:::: DirectoryServices.ActiveDirectory ::::

Directory services namespace, C# objects, methods and classes exist for public distribution on MSDN/Technet - amnyone can use them - it's just that normally not many people do because scripting is much less painful.

C# Directory Services project was quickly researched and executed while I was the AD technical Lead and senior systems engineer consulting for the main IT provider in Asia-Pacific region, during a project for a major Australian icon airline. 

Some notes as as rationale:

:::: DS C# Notes ::::

PATHS ATTEMPTED for subnet creation automation

::::: 1. VBScript
NOT FULLY WORKING - very cumbersome

::::: 2. PowerShell
NOT FULLY WORKING 

LOGs
Using PowerShell ISE for editing
DCPROMO of domain controller with W2K8 for PS scripting (unrestricted signing etc.), still not working Some issues with script signing, then still non full access after dcpromo and raise to ent admin level

:::: 3. C# and batch CMD --- OK ---

Installation of VS2010 premium
Using DirectoryServices classes inbuilt in the latest .Net Framework - will probably need to install latest .Net Framework/s on test VM

#####

C# NOTES - last relevant to topology (sites,subnets,etc.):

Inside main program entery for C# console apps, program.cs, added a createsubnet method which also takes a fourth argument (before description was not considered by parameter and created automatically with string concatenation):

Added method:

	case "createsubnetplus":
               try
                {
                   MngTopology.CreateSubnetPlus(args[1], args[2], args[3],args[4]);
          	}
                catch (IndexOutOfRangeException)
                {
                   Console.WriteLine("The syntax is: CreateSubnet newSubnet subnetLocationName siteName\n\n" +
                }                        }
                break;


Also added new method inside TopologyManagement.cs - method name:

public static void CreateSubnetPlus(string subNetName, string subNetLocation, string siteName, string siteDescription)

Inside this method description is not added with string concatenation of subNetLocation, subNetName and siteName:

 //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;

BUT it's now passed directly as an argument, such as:

de.Properties["description"].Value = siteDescription;

method snippet:

public static void CreateSubnetPlus(string subNetName, string subNetLocation, string siteName, string siteDescription)
        {
            // create a new subnet wiht a description
            try
            {

                DirectoryContext forestContext = new DirectoryContext(DirectoryContextType.Forest);

                // get a site. The subnet will be assigned to it later.
                ActiveDirectorySite site = ActiveDirectorySite.FindByName(forestContext,siteName);

                // get a subnet using the specified directory context
                // and an IP with length of network mask written as: x.x.x.x/x
                // e.g., 10.1.1.0/24)
                ActiveDirectorySubnet subnet = new ActiveDirectorySubnet(forestContext,subNetName);

                // set the location of this subnet
                subnet.Location = subNetLocation;

                // set the site to which this subnet is a member
                subnet.Site = site;

                // save the subnet to the directory
                subnet.Save();
                Console.WriteLine("\nSubnet \"{0}\" was created successfully", subnet);

                // get the subnet from the directory
                DirectoryEntry de = subnet.GetDirectoryEntry();

                // set the description. Currently, this is not exposed as a property of the
                // ActiveDirectorySubnet object.

                //de.Properties["description"].Value = subNetLocation + "
(" + subNetName + ") in " + siteName;
                de.Properties["description"].Value = siteDescription;

                // save the change back to the directory
                de.CommitChanges();

            }

#####

Subnets are exported to csv then simply processed by c# program with a short shell CMD to process all tokens inside the csv file

CMD is:  

C:\code\C#\DirectoryServices.ActiveDirectory\obj\Debug>for /f "tokens=1,2,3,4 delims=," %a in (AD_Subnets3.csv) do AD_shell.exe createsubnetplus %a %b %c %d

CSV files are produced from a DHCP report coming from Telstra - main telecommunication provider in Australia

I.e. following will run through all entries in csv file and search for them, returning the main attributes in the shell if subnets are found.

for /f "tokens=1,2 delims=," %a in (SE_Asia_subnets.csv) do AD_shell_v1.12.exe findsubnet %a %b

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

NEW methods I created

EDITING
creation and modification of AD Sites & Services attributes

public static void CreateSubnetPlus(string subNetName, string subNetLocation, string siteName, string siteDescription)

public static void modifySubnetDescNoFor(string subNetName, string subnetDescription)

public static void modifySubnetDesc(string targetForestName, string subNetName, string subnetDescription)

public static void modifySubnetLoc(string targetForestName, string subNetName, string subnetLocation)

...abbreviated: modifySubnetLoc2, ModifySLName, ModifySLDesc, ModifySiteDesc

REPORTING

FindSubnet, FindSubnetByLoc, FindSiteLink, FindSiteLink2, FindSite, FindSite2
