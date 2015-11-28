# az1
aztest

Started with aztest - a test of course

Branching...

First addition is a C# project in the Directory Services area - I am NOT a software developer so my code is potentially quite raw in certain areas of the namespace/s, but as I am essentially an infrastructure/security engineer, it quite relentlessly always "does the job".

:::: DirectoryServices.ActiveDirectory ::::

C# Directory Services project was quickly researched and executed as senior systems engineer for a major ariline, while  consulting for the main IT provider in Asia-Pacific region.

Some notes as as rationale:

:::: DS C# Notes ::::

PATHS ATTEMPTED for subnet creation automation

::::::::::::::::::::
1.
VBScript
NOT FULLY WORKING - very cumbersome

::::::::::::::::::::
2.
PowerShell
NOT FULLY WORKING 

LOGs
Using PowerShell ISE for editing
DCPROMO of domain controller with W2K8 for PS scripting (unrestricted signing etc.), still not working Some issues with script signing, then still non full access after dcpromo and raise to ent admin level

::::::::::::::::::::

3.
C# and batch CMDs
OK

Installation of VS2010 premium
Using DirectoryServices classes inbuilt in the latest .Net Framework - will probably need to install latest .Net Framework/s on SYDHDCMS97-EUC

####################

C# NOTES - last relevant to topology (sites,subnets,etc.):

Inside main program entery for C# console apps, program.cs, added a createsubnet method which also takes a fourth argument (before description was not considered by parameter and created autoatically with string
concatenation):
Added method

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


Inside TopologyManagement.cs added new method

public static void CreateSubnetPlus(string subNetName, string subNetLocation, string siteName, string siteDescription)

Inside this method description is not added with string concatenation of subNetLocation, subNetName and siteName:

 //de.Properties["description"].Value = subNetLocation + " (" + subNetName
+ ") in " + siteName;

BUT it's now passed directly as an argument:

de.Properties["description"].Value = siteDescription;


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

####################

Subnets are exported to csv then simply processed by c# program with a short shell CMD to process all tokens inside the csv file

CMD is:  

C:\code\C#\DirectoryServices.ActiveDirectory\obj\Debug>for /f "tokens=1,2,3,4 delims=," %a in (AD_Subnets3.csv) do AD_shell.exe createsubnetplus %a %b %c %d

CSV files are prodcued from a DHCP report coming from Telstra - main telecommunication provider in Australia

I.e. following will run through all entries in csv file and search for them in 

for /f "tokens=1,2 delims=," %a in (Asia_subnets.csv) do AD_shell_v1.12.exe findsubnet %a %b

