/*=====================================================================
  File:     ReplicationManagement.cs

  Summary:  Section for replication management related classes and 
            methods.
=====================================================================
                         **                         **
*/

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]

namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    class ReplicationManagement
    {

        public static void ReplicateFromSource(string sourceServer, string targetServer, string partitionName)
        {
            try
            {

                // set a directory server context for the target server
                DirectoryContext targetContext = new DirectoryContext(
                                                    DirectoryContextType.DirectoryServer,
                                                    targetServer);

                // bind to a specific dc to serve as the replication source
                DomainController targetDc =
                                    DomainController.GetDomainController(targetContext);


                // invoke the kcc to check the replication topology of the target dc
                targetDc.CheckReplicationConsistency();
                Console.WriteLine("\nReplication topology is consistent.\n");


                // trigger a synchornization of a replica from a source dc
                // to the target dc
                targetDc.SyncReplicaFromServer(partitionName, sourceServer);

                Console.WriteLine("\nSynchronize partition \"{0}\" " +
                                    "from server {1} to {2} succeed",
                                    partitionName,
                                    sourceServer,
                                    targetServer);

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n{0}:{1}",
                                  e.GetType().Name, e.Message);
            }

        }

        public static void ReplicateFromNeighbors(string targetServer, string partitionName)
        {

            try
            {
                // set a directory server context for the target server
                DirectoryContext targetContext = new DirectoryContext(
                                                    DirectoryContextType.DirectoryServer,
                                                    targetServer);

                // bind to a specific domain controller to serve as the 
                // source of a replication connection
                DomainController targetDc =
                                    DomainController.GetDomainController(targetContext);


                // invoke the kcc to check the replication topology for the 
                // target domain controller
                targetDc.CheckReplicationConsistency();
                Console.WriteLine("\nReplication topology is consistent.\n");


                // sync replica from all neighbors
                targetDc.TriggerSyncReplicaFromNeighbors(partitionName);
                Console.WriteLine("\nTriggered a synchronization of partition {0} " +
                                  "to {1} from all neighbors succeeded.", partitionName, targetServer);

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n{0}:{1}",
                                  e.GetType().Name, e.Message);
            }

        }

        public static void SyncAllServers(string partitionName)
        {
            try
            {
                // get a domain context
                DirectoryContext context = new DirectoryContext(
                                                DirectoryContextType.Domain);

                // bind to an available domain controller in the domain.
                DomainController dc = DomainController.FindOne(context);

                // invoke the kcc to check the replication topology for the 
                // current domain controller
                dc.CheckReplicationConsistency();
                Console.WriteLine("\nCheck replication consistency succeed\n");

                // Set the name of the SyncUpdateCallback delegate for this dc
                // see the SyncFromAllServersCallbackDelegate routine
                dc.SyncFromAllServersCallback = SyncFromAllServersCallbackDelegate;

                Console.WriteLine("\nStart sync with all servers:");

                // Call the synch. method and set synch. options 
                dc.SyncReplicaFromAllServers(partitionName,
                                    SyncFromAllServersOptions.AbortIfServerUnavailable
                                    | SyncFromAllServersOptions.CrossSite
                                    );

                Console.WriteLine("\nSynchronize partition \"{0}\" " +
                                  "with all servers succeeded", partitionName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n{0}:{1}",
                                  e.GetType().Name, e.Message);
            }

        }

        // This SyncUpdateCallback delegate receives event
        // notifications during replica synchronization
        private static bool SyncFromAllServersCallbackDelegate(
                                SyncFromAllServersEvent eventType,
                                string targetServer,
                                string sourceServer,
                                SyncFromAllServersOperationException e
                            )
        {
            // return the type of synchronization event that occured
            Console.WriteLine("\neventType is {0}", eventType);

            // return the DN of the target nTDSDSA object for replication
            // this will be null when the eventType reports finished
            if (targetServer != null)
                Console.WriteLine("target is {0}", targetServer);

            // return the DN of the source nTDSDSA object for replication
            // this will be null when the eventType reports finished
            if (sourceServer != null)
                Console.WriteLine("source is {0}", sourceServer);

            // return any sync. operation exception
            // this will be null if there is no exception to report
            if (e != null)
                Console.WriteLine("exception is {0}", e);

            // return true to instruct the calling method to
            // continue its operation. The SyncUpdateCallback 
            // delegate returns false when the eventType is finsihed.
            return true;
        }


        public static void CreateNewConnection(string sourceServer, string targetServer, string connectionName)
        {
            try
            {

                // set a directory server context for the source server
                DirectoryContext sourceContext = new DirectoryContext(
                                                    DirectoryContextType.DirectoryServer,
                                                    sourceServer);

                // set a directory server context for the target server
                DirectoryContext targetContext = new DirectoryContext(
                                                    DirectoryContextType.DirectoryServer,
                                                    targetServer);


                // bind to a specific domain controller to serve as the 
                // source of a replication connection
                DomainController sourceDc =
                                    DomainController.GetDomainController(sourceContext);

                ReplicationConnection connection = new ReplicationConnection(
                                                        targetContext,
                                                        connectionName,
                                                        sourceDc);

                // set change notification status
                connection.ChangeNotificationStatus = NotificationStatus.IntraSiteOnly;


                // create a customized replication schedule
                ActiveDirectorySchedule schedule = new ActiveDirectorySchedule();
                schedule.SetDailySchedule(HourOfDay.Twelve,
                                          MinuteOfHour.Zero,
                                          HourOfDay.Fifteen,
                                          MinuteOfHour.Zero);

                schedule.SetSchedule(DayOfWeek.Sunday,
                                     HourOfDay.Eight,
                                     MinuteOfHour.Zero,
                                     HourOfDay.Eleven,
                                     MinuteOfHour.Zero);

                schedule.SetSchedule(DayOfWeek.Saturday,
                                     HourOfDay.Seven,
                                     MinuteOfHour.Zero,
                                     HourOfDay.Ten,
                                     MinuteOfHour.Zero);

                connection.ReplicationSchedule = schedule;
                connection.ReplicationScheduleOwnedByUser = true;
                
                // save the new connection to the directory
                connection.Save();
                Console.WriteLine("\nNew replication connection created successfully\n" +
                  "from server {0} to {1}.\n The connection appears in the NTDS " +
                  "settings of {1}", sourceServer, targetServer);

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n{0}:{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        public static void SetReplicationConnection(string server, string connectionName)
        {

            try
            {

                DirectoryContext context = new DirectoryContext(
                                                    DirectoryContextType.DirectoryServer,
                                                    server);

                ReplicationConnection connection =
                                            ReplicationConnection.FindByName(
                                                                     context,
                                                                     connectionName);

                Console.WriteLine("\nGet replication connection \"{0}\" information:",
                                    connection.Name);

                Console.WriteLine("ChangeNotificationStatus is {0}",
                                                   connection.ChangeNotificationStatus);

                if (connection.ChangeNotificationStatus != NotificationStatus.NoNotification)
                {
                    // this changes the options attribute value of the nTDSConnection object.
                    connection.ChangeNotificationStatus = NotificationStatus.NoNotification;
                    Console.WriteLine("ChangeNotificationStatus set to no notification");
                    connection.Save();
                }
                else
                {
                    Console.WriteLine("ChangeNotificationStatus was not changed. It was " +
                        "already set to no notification");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n{0}:{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        public static void DeleteReplicationConnection(string server, string connectionName)
        {

            try
            {

                DirectoryContext context = new DirectoryContext(
                                                DirectoryContextType.DirectoryServer,
                                                server);

                ReplicationConnection connection =
                                            ReplicationConnection.FindByName(
                                                                     context,
                                                                     connectionName);

                // delete the replication connection
                connection.Delete();
                Console.WriteLine("\nReplication connection {0} deleted", connectionName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n{0}:{1}",
                                  e.GetType().Name, e.Message);
            }

        }

    }

}
