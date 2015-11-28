/*=====================================================================
  File:     TopologyManagement.cs

  Summary:  Demonstrates managing directory topology objects including
            sites, subnets, bridgeheads and sitelinks.
=====================================================================
                       
*/

using System;

using System.Security;

using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]


namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    class MngTopology
    {
        
        public static void CreateAdSite(string siteName)
        {
            try
            {

                // get a forest context
                DirectoryContext forestContext = new DirectoryContext(
                                        DirectoryContextType.Forest);

                // create a new site
                ActiveDirectorySite site = new ActiveDirectorySite(forestContext,
                                                                   siteName);

                // set site options
                site.Options = ActiveDirectorySiteOptions.GroupMembershipCachingEnabled;

                // commit the site to the directory
                site.Save();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSite \"{0}\" was created successfully", site);
                Console.ResetColor();

            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }

        }


        public static void CreateAdamSite(string targetName, string newSiteName)
        {

            try
            {
                // assemble the connection string using the host name and the port assigned to ADAM
                string adamConnectionString = targetName;

                DirectoryContext adamContext = new DirectoryContext(
                                                    DirectoryContextType.DirectoryServer,
                                                    adamConnectionString);

                ActiveDirectorySite site = new ActiveDirectorySite(adamContext,
                                                                   newSiteName);

                // set site options
                site.Options = ActiveDirectorySiteOptions.GroupMembershipCachingEnabled;

                // commit the site to the directory
                site.Save();
                Console.WriteLine("\nSite \"{0}\" was created successfully", site);

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        public static void CreateSubnet(string subNetName, string subNetLocation, string siteName)
        {
            // create a new subnet
            try
            {

                DirectoryContext forestContext = new DirectoryContext(
                                            DirectoryContextType.Forest);

                // get a site. The subnet will be assigned to it later.
                ActiveDirectorySite site = ActiveDirectorySite.FindByName(
                                                forestContext,
                                                siteName);

                // get a subnet using the specified directory context 
                // and an IP with length of network mask written as: x.x.x.x/x
                // e.g., 10.1.1.0/24)
                ActiveDirectorySubnet subnet = new ActiveDirectorySubnet(
                                                                forestContext,
                                                                subNetName);

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
                
                de.Properties["description"].Value = subNetLocation +
                    " (" + subNetName + ") in " + siteName;

                // save the change back to the directory
                de.CommitChanges();

            }
            catch (ActiveDirectoryOperationException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: {0}\n{1}",
                                    e.GetType().Name, e.Message);
            }

        }

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
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSubnet \"{0}\" was created successfully", subnet);
                // Restore the original foreground and background colors.
                Console.ResetColor();
                // get the subnet from the directory 

                DirectoryEntry de = subnet.GetDirectoryEntry();

                // set the description. Currently, this is not exposed as a property of the
                // ActiveDirectorySubnet object.

                //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                de.Properties["description"].Value = siteDescription;

                // save the change back to the directory
                de.CommitChanges();

            }
            catch (ActiveDirectoryOperationException e)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return;
            }

            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unexpected exception: {0}\n{1}",e.GetType().Name, e.Message);
                Console.ResetColor();
            }

        }

        public static void modifySubnetDescNoFor(string subNetName, string subnetDescription)
        {
            try
            {
                //DirectoryContext forestContext = new DirectoryContext(DirectoryContextType.Forest);
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        if (subnet.Name.Equals(subNetName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\t\tSubnet: {0} location: {1}", subnet.Name, subnet.Location);

                            //ActiveDirectorySubnet subnet2 = ActiveDirectorySubnet.FindByName(context, subNetName);
                            DirectoryEntry de = subnet.GetDirectoryEntry();

                            // set the description. Currently, this is not exposed as a property of the
                            // ActiveDirectorySubnet object.

                            //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                            de.Properties["description"].Value = subnetDescription;

                            
                            subnet.Save();
                            de.CommitChanges();
                            //subnet.Save();
                            Console.ResetColor();
                        }

                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Not working????");
                            //Console.ResetColor();
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void modifySubnetDesc(string targetForestName, string subNetName, string subnetDescription)
        {
            try
            {
                //DirectoryContext forestContext = new DirectoryContext(DirectoryContextType.Forest);
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                
                {

                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        if (subnet.Name.Equals(subNetName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("..modifying description for Subnet {0} ", subnet.Name);

                            //ActiveDirectorySubnet subnet2 = ActiveDirectorySubnet.FindByName(context, subNetName);
                            DirectoryEntry de = subnet.GetDirectoryEntry();

                            // set the description. Currently, this is not exposed as a property of the
                            // ActiveDirectorySubnet object.

                            //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                            de.Properties["description"].Value = subnetDescription;

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Subnet {0} ", subnet.Name + " description has been modified!");

                            //subnet.Save();
                            de.CommitChanges();
                            Console.ResetColor();
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void modifySubnetLoc(string targetForestName, string subNetName, string subnetLocation)
        {
            try
            {
                //DirectoryContext forestContext = new DirectoryContext(DirectoryContextType.Forest);
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        if (subnet.Name.Equals(subNetName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("..modifying location for Subnet {0} ", subnet.Name);

                            //ActiveDirectorySubnet subnet2 = ActiveDirectorySubnet.FindByName(context, subNetName);
                            DirectoryEntry de = subnet.GetDirectoryEntry();

                            // set the description. Currently, this is not exposed as a property of the
                            // ActiveDirectorySubnet object.

                            //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                            de.Properties["location"].Value = subnetLocation;

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Subnet {0} ", subnet.Name + " location has been modified!");
                            //subnet.Save();
                            de.CommitChanges();
                            Console.ResetColor();
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void modifySubnetLoc2(string targetForestName, string comp1, string comp2)
        {
            try
            {
                //DirectoryContext forestContext = new DirectoryContext(DirectoryContextType.Forest);
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        if (subnet.Location.Contains(comp1))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("..modifying location for Subnet {0} ", subnet.Name);

                            //ActiveDirectorySubnet subnet2 = ActiveDirectorySubnet.FindByName(context, subNetName);
                            DirectoryEntry de = subnet.GetDirectoryEntry();

                            // set the description. Currently, this is not exposed as a property of the
                            // ActiveDirectorySubnet object.

                            //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                            de.Properties["location"].Value = comp2;

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Subnet {0} ", subnet.Name + " location has been modified!");
                            //subnet.Save();
                            de.CommitChanges();
                            Console.ResetColor();
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void ModifySLName(string targetForestName, string SiteLinkName, string SLNewName)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySiteLink sitelink in site.SiteLinks)
                    {
                        if (sitelink.Name.Equals(SiteLinkName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("..modifying name for SiteLink {0} ", sitelink.Name);

                            //ActiveDirectorySubnet subnet2 = ActiveDirectorySubnet.FindByName(context, subNetName);
                            DirectoryEntry de = sitelink.GetDirectoryEntry();

                            // set the description. Currently, this is not exposed as a property of the
                            // ActiveDirectorySubnet object.

                            //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                            //de.Properties["name"].Value = SLNewName;
                            SiteLinkName.Replace(SiteLinkName, SLNewName);
                            //sitelink.Name.Replace(SiteLinkName, SLNewName);
                            sitelink.Save();
                            //de.Name.Replace(SiteLinkName, SLNewName);
                            Console.WriteLine(SiteLinkName);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Sitelink {0} ", sitelink.Name + " name has been modified!");

                            //subnet.Save();
                            de.CommitChanges();
                            Console.ResetColor();


                        }

                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void ModifySLDesc(string targetForestName, string SiteLinkName, string SLDescription)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySiteLink sitelink in site.SiteLinks)
                    {
                        if (sitelink.Name.Equals(SiteLinkName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("..modifying description for SiteLink {0} ", sitelink.Name);

                            //ActiveDirectorySubnet subnet2 = ActiveDirectorySubnet.FindByName(context, subNetName);
                            DirectoryEntry de = sitelink.GetDirectoryEntry();

                            // set the description. Currently, this is not exposed as a property of the
                            // ActiveDirectorySubnet object.

                            //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                            de.Properties["description"].Value = SLDescription;

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Sitelink {0} ", sitelink.Name + " description has been modified!");

                            //subnet.Save();
                            de.CommitChanges();
                            Console.ResetColor();


                        }

                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void ModifySiteDesc(string targetForestName, string SiteName, string SiteDesc)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);



                foreach (ActiveDirectorySite site in forest.Sites)
                {


                        if (site.Name.Equals(SiteName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("..modifying description for SiteLink {0} ", site.Name);

                            //ActiveDirectorySubnet subnet2 = ActiveDirectorySubnet.FindByName(context, subNetName);
                            DirectoryEntry de = site.GetDirectoryEntry();

                            // set the description. Currently, this is not exposed as a property of the
                            // ActiveDirectorySubnet object.

                            //de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName;
                            de.Properties["description"].Value = SiteDesc;

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Site {0} ", site.Name + " description has been modified!");

                            //subnet.Save();
                            de.CommitChanges();
                            Console.ResetColor();
                        }

                    
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        /*
        public static void FindSubnet(string subNetName)
        {
            Console.WriteLine("Enter forest and subnet name");
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, "azeta.com");
            Forest forest = Forest.GetForest(context);
            Console.WriteLine(forest);

            foreach (ActiveDirectorySite site in forest.Sites)
            {
                foreach (ActiveDirectorySubnet subnet in site.Subnets)
                {
                    {
                        if (subnet.Name.Equals(subNetName))
                        {
                            Console.WriteLine("\t\tSubnet: {0} location: {1}", subnet.Name, subnet.Location);
                        }
                    }
                }
            }
        }
        */
        
        public static void FindSubnet(string targetForestName, string subNetName)
        {
            try
            {
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                //Console.WriteLine("Forest context is: " + forest);
                
                int counter = 0;
                // int max = 0;
                // int i = 0;

                //int[] nums = new int[22];
                ArrayList num = new ArrayList();

                foreach (ActiveDirectorySite site in forest.Sites)
                {
                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        if (subnet.Name.Equals(subNetName))
                        {
                            //int counter = 0;
                            //counter++;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Found subnet "+subnet.Name);
                            Console.ResetColor();
                            /*
                            for (i = 0; i < num.Capacity; i++)
                            {
                                num[i] = counter;
                                Console.WriteLine(i);
                                Console.WriteLine(num[i]);
                                Console.WriteLine(counter);
                            }
                            */
                            //int max = 0;
                            
                            // FRRRRRRRRRRRRRR
                            
                            /*
                            for (i = 0; i < num.Capacity; i++)
                            {
                                if (num.Capacity > max)
                                {
                                    max = i;
                                }
                                //Console.WriteLine("This is i "+i);
                                //Console.WriteLine("This is max " + max);
                                
                            }
                            */

                            // FRRRRRRRRRRRRRR

                            //SubnetFound = subnet;
                            /*
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Subnet found!" + "\nSubnet: {0} \nLocation: {1}\nSite: {2}", subnet.Name, subnet.Location,subnet.Site);
                            Console.WriteLine(i);
                            Console.ResetColor();
                            Console.WriteLine(num[i]);
                             */
                         }
                        counter++;
                       
                    }
                    
                }
                //counter--;
                Console.WriteLine("There are " + counter + " subnets in total");
            }
                
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void FindSubnetByLoc(string targetForestName, string subNetLoc)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        if (subnet.Location.Equals(subNetLoc))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Subnet found!" + "\tSubnet: {0} location: {1}", subnet.Name, subnet.Location);
                            Console.ResetColor();
                        }


                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void FindSiteLink(string targetForestName, string SiteLinkName)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySiteLink sitelink in site.SiteLinks)
                    {
                        if (sitelink.Name.Equals(SiteLinkName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("SiteLink found!" + " SiteLink: {0} ", sitelink.Name);
                            Console.ResetColor();
                        }

                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void FindSiteLink2(string targetForestName, string SiteLinkName)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySiteLink sitelink in site.SiteLinks)
                    {
                        if (sitelink.Name.Equals(SiteLinkName))
                        {

                            ActiveDirectoryInterSiteTransport transport = ActiveDirectoryInterSiteTransport.FindByTransportType(context, ActiveDirectoryTransportType.Rpc);

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("SiteLink found!" + " SiteLink: {0} ", sitelink.Name);
                            Console.WriteLine("It has these many sites " + sitelink.Sites.Count);
                            Console.WriteLine("It has these many KAZZYYY " + sitelink.Sites);
                            Console.ResetColor();

                            /*
                            foreach (ActiveDirectorySite siteOfLink in sitelink.Sites)
                            {
                                Console.WriteLine("Pooo" + " siteOflink : {0}) " + 
                            }

                                */
                        }

                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void FindSite(string targetForestName, string SiteName)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                //Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    if (site.Name.Equals(SiteName))
                        {

                         
                           ActiveDirectoryInterSiteTransport transport = ActiveDirectoryInterSiteTransport.FindByTransportType(context, ActiveDirectoryTransportType.Rpc);
                           Console.WriteLine("\tIgnore replication schedule" + "for this inter-site transport? {0}", transport.IgnoreReplicationSchedule);
                           // Console.WriteLine("\tIgnore replication schedule" + "for this inter-site transport? {0}", 
                           Console.WriteLine("\tBridge all site links {0}", transport.BridgeAllSiteLinks);

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Site found!" + " Site: {0} ", site.Name);

                            Console.ForegroundColor = ConsoleColor.Gray;
                        // FUUUUUUUUUUU

                            Console.WriteLine("\nSite \"{0}\"", site.Name + " contains the following domain(s):");
                            foreach (Domain domain in site.Domains)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\t{0}", domain.Name);
                            }
                        
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("It contains the following server(s):");
                            foreach (DirectoryServer server in site.Servers)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\t{0}", server.Name);
                            }

                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("And it contains the following subnet(s):");
                            foreach (ActiveDirectorySubnet subnet in site.Subnets)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\tSubnet: {0} location: {1}", subnet.Name, subnet.Location);
                            }

                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nInterSiteTopologyGenerator is {0}", site.InterSiteTopologyGenerator);

                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nBridgehead servers:");
                            foreach (DirectoryServer server in site.BridgeheadServers)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("\t\t{0}", server.Name);
                            }

                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nPreferred Bridgehead servers:");
                            foreach (DirectoryServer server in site.PreferredRpcBridgeheadServers)
                            {
                                Console.WriteLine("\t\t{0}", server.Name);
                            }
                        

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            //Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nAnd it is part of this/these Site Link/s:");
                            foreach (ActiveDirectorySiteLink sitelink in site.SiteLinks)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\t\t{0}", sitelink.Name);
                            }


                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.ResetColor();
                            Console.WriteLine("\t\t{0}ECHO complete");
                        }

                    
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void FindSite2(string targetForestName, string SiteName)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                //Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {
                   if (site.Name.Equals(SiteName))
                    {
                        ActiveDirectoryInterSiteTransport transport = ActiveDirectoryInterSiteTransport.FindByTransportType(context, ActiveDirectoryTransportType.Rpc);
                        Console.WriteLine("\tIgnore replication schedule" + "for this inter-site transport? {0}", transport.IgnoreReplicationSchedule);
                        // Console.WriteLine("\tIgnore replication schedule" + "for this inter-site transport? {0}", 
                        Console.WriteLine("\tBridge all site links {0}", transport.BridgeAllSiteLinks);

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Site found!" + " Site: {0} ", site.Name);

                        Console.ForegroundColor = ConsoleColor.Gray;
                        // FUUUUUUUUUUU

                        Console.WriteLine("\nSite \"{0}\"", site.Name + " contains the following domain(s):");
                        foreach (Domain domain in site.Domains)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\t{0}", domain.Name);
                        }

                        /*
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("It contains the following server(s):");
                        foreach (DirectoryServer server in site.Servers)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\t{0}", server.Name);
                        }
                        */

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("And it contains the following subnet(s):");
                        foreach (ActiveDirectorySubnet subnet in site.Subnets)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\tSubnet: {0} location: {1}", subnet.Name, subnet.Location);
                        }

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\nInterSiteTopologyGenerator is {0}", site.InterSiteTopologyGenerator);

                        /*
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\nBridgehead servers:");
                        foreach (DirectoryServer server in site.BridgeheadServers)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\t\t{0}", server.Name);
                        }
                        */

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\nPreferred Bridgehead servers:");
                        foreach (DirectoryServer server in site.PreferredRpcBridgeheadServers)
                        {
                            Console.WriteLine("\t\t{0}", server.Name);
                        }


                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        //Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\nAnd it is part of this/these Site Link/s:");
                        foreach (ActiveDirectorySiteLink sitelink in site.SiteLinks)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\t\t{0}", sitelink.Name);
                        }


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.ResetColor();
                        Console.WriteLine("\t\t{0}ECHO complete");
                    }


                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void CreateSubnetFromCSV(string subNetName, string subNetLocation, string siteName)
        {
            // create a new subnet
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

                // set the description. Currently, this is not exposed as a property of the ActiveDirectorySubnet object
                de.Properties["description"].Value = subNetLocation + " (" + subNetName + ") in " + siteName; 
                // save the change back to the directory
                de.CommitChanges();

            }
            catch (ActiveDirectoryOperationException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: {0}\n{1}",
                                    e.GetType().Name, e.Message);
            }

        }

        // How to create a site link. Note, that this example assigns a single site to this site link
        public static void CreateSiteLink(string siteName, string siteLinkName)
        {
            try
            {
                DirectoryContext forestContext = new DirectoryContext(
                                            DirectoryContextType.Forest);

                //bind to a specific site in the forest
                ActiveDirectorySite site = ActiveDirectorySite.FindByName(
                                                                forestContext,
                                                                siteName);

                // rpc is the default transport type (smtp is the other, 
                // this is how you can create a transport type variable to 
                // later assign to the link
                ActiveDirectoryTransportType adTpt =
                                            ActiveDirectoryTransportType.Rpc;

                // using the overload requiring the transport type to  
                // demonstrate how to assign a transport type to this link.
                ActiveDirectorySiteLink link = new ActiveDirectorySiteLink(
                                                    forestContext,
                                                    siteLinkName,
                                                    adTpt);

                // configure the ActiveDirectorySiteLink object 
                link.Cost = 100;
                link.DataCompressionEnabled = true;

                // create an AD schedule object for setting intersite replication
                ActiveDirectorySchedule linkSchedule =
                                                new ActiveDirectorySchedule();

                // set the schedule for 5:30 am to 6:30 am
                linkSchedule.SetDailySchedule(HourOfDay.Zero,
                                              MinuteOfHour.Zero,
                                              HourOfDay.TwentyThree,
                                              MinuteOfHour.FortyFive);

                // set the schedule for 5:30 pm to 6:30 pm
                /*linkSchedule.SetDailySchedule(HourOfDay.Seventeen,
                                              MinuteOfHour.Thirty,
                                              HourOfDay.Eighteen,
                                              MinuteOfHour.Thirty);
                */
                // apply the replication schedule to the link
                link.InterSiteReplicationSchedule = linkSchedule;

                // enable inter-site change notification. Typically used for fast, 
                // uncongested links between sites 
                link.NotificationEnabled = true;


                // replicate every twelve hours
                TimeSpan linkTimeSpan = new TimeSpan(0, 15, 0);

                // assign the TimeSpan object to the ReplicationInterval property
                link.ReplicationInterval = linkTimeSpan;

                // configure the link so there is no reciprocal replication
                link.ReciprocalReplicationEnabled = false;

                // assign a site to this site link. 
                link.Sites.Add(site);

                // commit the link to the directory
                link.Save();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nLink \"{0}\" was created successfully", link.Name);
                Console.ResetColor();

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                      e.GetType().Name, e.Message);

            }

        }


        public static void AddSiteToSiteLink(string siteName, string siteLinkName)
        {
            // add new site to an existing site link
            try
            {
                // get the forest context
                DirectoryContext forestContext = new DirectoryContext(
                                        DirectoryContextType.Forest);

                // bind to a specific site
                ActiveDirectorySite site = ActiveDirectorySite.FindByName(
                                                                    forestContext,
                                                                    siteName);

                // bind to a specific site link
                ActiveDirectorySiteLink link = ActiveDirectorySiteLink.FindByName(
                                                                        forestContext,
                                                                        siteLinkName);

                
                Console.WriteLine("\nAdd site \"{0}\" to site link \"{1}\"", site.Name,
                                                                             link.Name);
                // add the site to the site link
                link.Sites.Add(site);

                // commit the change to the directory
                link.Save();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSiteLink \"{0}\" now contains: ", link);
                Console.ResetColor();

                foreach (ActiveDirectorySite s in link.Sites)
                {
                    Console.WriteLine("\tSite \"{0}\"", s);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }

        }


        public static void RemoveSiteFromSiteLink(string siteName, string siteLinkName)
        {
            try
            {
                // get a forest context
                DirectoryContext forestContext = new DirectoryContext(
                                DirectoryContextType.Forest);

                // bind to a site using FindByName and the forest context
                ActiveDirectorySite site = ActiveDirectorySite.FindByName(
                                                                    forestContext,
                                                                    siteName);

                // bind to a site link using FindByName and the forest context
                ActiveDirectorySiteLink link = ActiveDirectorySiteLink.FindByName(
                                                                    forestContext,
                                                                    siteLinkName);

                //remove the site from the site link
                link.Sites.Remove(site);

                // commit the change to the directory
                link.Save();

                Console.WriteLine("\nSite {0} removed from site link {1}", site, link);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }

        }

        public static void MoveDcToSite(string sourceDC, string targetSite)
        {

            try
            {
                // get a dc to move
                DirectoryContext dcContext = new DirectoryContext(
                                            DirectoryContextType.DirectoryServer, sourceDC);

                DomainController dc = DomainController.GetDomainController(dcContext);

                dc.MoveToAnotherSite(targetSite);

                Console.WriteLine("{0} succesfully moved to {1}", sourceDC, targetSite);
            }
            catch (Exception e)
            {

                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                  e.GetType().Name, e.Message);

            }

        }


        public static void DeleteSubnet(string targetForestName, string subNetName)
        {
            try
            {

                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest, targetForestName);
                Forest forest = Forest.GetForest(context);
                Console.WriteLine("Forest context is: " + forest);

                foreach (ActiveDirectorySite site in forest.Sites)
                {

                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        if (subnet.Name.Equals(subNetName))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Deleting subnet: {0} in Site: {1}", subnet.Name, subnet.Site);
                            subnet.Delete();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Subnet Deleted!");
                            Console.ResetColor();
                        }

                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void DeleteAdSite(string siteName)
        {
            try
            {
                // get a forest context
                DirectoryContext forestContext = new DirectoryContext(
                                DirectoryContextType.Forest);

                // bind to a site using FindByName and the forest context
                ActiveDirectorySite site = ActiveDirectorySite.FindByName(forestContext, siteName);

                // iterate the subnets in the site object and call
                // the Delete method on each subnet. 
                foreach (ActiveDirectorySubnet subnet in site.Subnets)
                {
                    subnet.Delete();
                }

                // delete the site 
                site.Delete();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSite and subnets were deleted successfully\n");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }

        }


        public static void DeleteAdamSite(string targetName, string siteName)
        {

            try
            {

                // assemble the connection string using the host name and the port assigned to ADAM
                string adamConnectionString = targetName;

                // get an ADAM context by connecting to the server running ADAM
                DirectoryContext adamSrvrContext = new DirectoryContext(
                                                        DirectoryContextType.DirectoryServer,
                                                        targetName);

                // bind to a specific ADAM site
                ActiveDirectorySite site =
                        ActiveDirectorySite.FindByName(adamSrvrContext, siteName);

                //delete the site
                site.Delete();

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        public static void DeleteLink(string linkName)
        {
            try
            {
                // get a forest context
                DirectoryContext forestContext = new DirectoryContext(
                                DirectoryContextType.Forest);

                // bind to a site using FindByName and the forest context
                ActiveDirectorySiteLink link = ActiveDirectorySiteLink.FindByName(
                                                                        forestContext,
                                                                        linkName);

                //delete the link 
                link.Delete();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nLink {0} was deleted successfully\n", link.Name);
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }

        }

    }
}