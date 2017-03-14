:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

Adding Directory Services C# project files

Historical notes

The AD Directory Services C# console was started during a large infrastructure project back in 2012

It played a part in automating some Active Directory tasks, mainly in AD sites and services, bulk modifications, creations, deletions of AD objects or some of their attributes. The console can also be used for reporting, which at the time was somewhat cumbersome in its builtin form (W2K3 and W2K8 server console)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

C# NOTES only about AD topology management (AD sites, subnets,etc.):

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




Subnets are exported to csv then simply processed by c# program with a short shell CMD to process all tokens inside the csv file

CMD is:  

C:\code\C#\DirectoryServices.ActiveDirectory\obj\Debug>for /f "tokens=1,2,3,4 delims=," %a in (AD_Subnets3.csv) do AD_shell.exe createsubnetplus %a %b %c %d

I.e. following will run through all entries in csv file and search for them, returning the main attributes in the shell if subnets are found.

for /f "tokens=1,2 delims=," %a in (SE_Asia_subnets.csv) do AD_shell_v1.12.exe findsubnet %a %b

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

Some of the custom methods created

EDITING
creation and modification of AD Sites & Services attributes

public static void CreateSubnetPlus(string subNetName, string subNetLocation, string siteName, string siteDescription)

public static void modifySubnetDescNoFor(string subNetName, string subnetDescription)

public static void modifySubnetDesc(string targetForestName, string subNetName, string subnetDescription)

public static void modifySubnetLoc(string targetForestName, string subNetName, string subnetLocation)

...abbreviated: modifySubnetLoc2, ModifySLName, ModifySLDesc, ModifySiteDesc

REPORTING 

FindSubnet, FindSubnetByLoc, FindSiteLink, FindSiteLink2, FindSite, FindSite2
