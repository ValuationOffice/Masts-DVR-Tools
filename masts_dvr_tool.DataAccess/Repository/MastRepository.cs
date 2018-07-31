using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.DataAccess.Repository
{
    public class MastRepository : IMastRepository
    {
        private string connectionString;
        private readonly string appRoleUsername;
        private readonly string appRolePassword;

        public MastRepository(string connectionString, string appRoleUsername, string appRolePassword)
        {
            //Workaround to address connectionection String issue
            if (connectionString.Contains("\\\\"))
                connectionString = connectionString.Replace("\\\\", "\\");

            this.connectionString = connectionString;
            this.appRoleUsername = appRoleUsername;
            this.appRolePassword = appRolePassword;
        }

        public Mast GetMastData(string sharing, string uid, string mastData = "0")
        {
            Mast mastDataObject = new Mast();
            string sql;

            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry($@"Sharing: {sharing}, UID: {uid}, MastData: {mastData}", EventLogEntryType.Information, 101, 1);
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_setapprole", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@rolename", appRoleUsername);

                    command.Parameters.AddWithValue("@password", appRolePassword);

                    connection.Open();
                    command.ExecuteNonQuery();
                }


                using (SqlCommand command = new SqlCommand("sp_GetTemplateData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Masts", SqlDbType.Int).Value = mastData;
                    command.Parameters.AddWithValue("@Sharing", SqlDbType.Int).Value = sharing;
                    command.Parameters.AddWithValue("@UID", SqlDbType.VarChar).Value = uid;

                    SqlDataReader reader = command.ExecuteReader();

                    int rowCount = 0;
                    while (reader.Read())
                    {
                        if (rowCount == 0) //Data is duplicated in numerous rows
                        {
                            mastDataObject.VOAAddressLine1 = reader["Address1"].ToString();
                            mastDataObject.VOAAddressLine2 = reader["Address2"].ToString();
                            mastDataObject.VOAAddressLine3 = reader["Address3"].ToString();
                            mastDataObject.VOAAddressLine4 = reader["Address4"].ToString();
                            mastDataObject.VOAPostcode = reader["Post Code"].ToString();
                            mastDataObject.VOACounty = reader["County"].ToString();
                            mastDataObject.VOAMastOperator = reader["Operator"].ToString();
                            mastDataObject.VOAEffectiveFrom = reader["EffectiveDate"].ToString();
                            mastDataObject.VOAShared = Convert.ToDouble(reader["NumberOfSharers"]) > 0 ? "Yes" : "No";
                            mastDataObject.VOARateableValue = $"£{reader["AdoptedRV"]}";
                            //Hard coding as all Masts are the same.
                            mastDataObject.VOAPrimaryDescription = "COMMUNICATION STATION AND PREMISES";

                        }

                        rowCount++;
                    }

                }
                
                if (mastDataObject.VOAShared == "Yes")
                {
                    int rowCount = 0;

                    sql = @"SELECT count(*) FROM [NBS2017_Masts].[dbo].[CMV2017D_Sharers] where UID = @UID";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UID", SqlDbType.VarChar);
                        command.Parameters["@UID"].Value = uid;

                        rowCount = (Int32)command.ExecuteScalar();

                    }

                    sql = @"SELECT * FROM [NBS2017_Masts].[dbo].[CMV2017D_Sharers] where UID = @UID";

                    StringBuilder sharers = new StringBuilder();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UID", SqlDbType.VarChar);
                        command.Parameters["@UID"].Value = uid;

                        SqlDataReader reader = command.ExecuteReader();

                        int count = 0;

                        while (reader.Read())
                        {
                            sharers.Append(reader["ItemName"]);
                            if (count < rowCount - 1)
                                sharers.Append(", ");

                            count++;
                        }


                    }
                    mastDataObject.VOASharedWith = sharers.ToString();
                }

                else
                {
                    mastDataObject.VOASharedWith = "N/A";
                }
            }
            return mastDataObject;
        }
    }
}
