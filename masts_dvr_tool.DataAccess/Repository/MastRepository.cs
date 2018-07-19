using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.DataAccess.Repository
{
    public class MastRepository : IMastRepository
    {
        private string connectionString;

        public MastRepository(string connectionString)
        {
            //Workaround to address Connection String issue
            if (connectionString.Contains("\\\\"))
                connectionString = connectionString.Replace("\\\\", "\\");

            this.connectionString = connectionString;
        }

        public Mast GetMastData(string sharing, string uid, string mastData = "0")
        {
            Mast mastDataObject = new Mast();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetTemplateData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Masts", SqlDbType.Int).Value = mastData;
                    command.Parameters.AddWithValue("@Sharing", SqlDbType.Int).Value = sharing;
                    command.Parameters.AddWithValue("@UID", SqlDbType.VarChar).Value = uid;

                    connection.Open();

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

                        }

                        rowCount++;
                    }

                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM [NBS2017_Masts].[dbo].[CMV2017D_Valuation] where UID = @UID";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@UID", SqlDbType.VarChar);
                    command.Parameters["@UID"].Value = uid;

                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        mastDataObject.VOARateableValue = $"£{reader["CalculatedRV"]}";
                    }


                }

            }

            if (mastDataObject.VOAShared == "Yes")
            {
                int rowCount = 0;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"SELECT count(*) FROM [NBS2017_Masts].[dbo].[CMV2017D_Sharers] where UID = @UID";



                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@UID", SqlDbType.VarChar);
                        command.Parameters["@UID"].Value = uid;

                        conn.Open();

                        rowCount = (Int32)command.ExecuteScalar();

                    }
                }


                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    string sql = @"SELECT * FROM [NBS2017_Masts].[dbo].[CMV2017D_Sharers] where UID = @UID";

                    StringBuilder sharers = new StringBuilder();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@UID", SqlDbType.VarChar);
                        command.Parameters["@UID"].Value = uid;

                        conn.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        int count = 0;

                        while (reader.Read())
                        {
                            sharers.Append(reader["ItemName"]);
                            if (count < rowCount -1)
                                sharers.Append(", ");

                            count++;
                        }


                    }
                    mastDataObject.VOASharedWith = sharers.ToString();
                }

            }

            else
            {
                mastDataObject.VOASharedWith = "N/A";
            }

            return mastDataObject;
        }
    }
}
