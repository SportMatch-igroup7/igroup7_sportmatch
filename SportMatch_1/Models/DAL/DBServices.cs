using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using SportMatch_1.Controllers;
using SportMatch_1.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // Insert Functions
    //--------------------------------------------------------------------------------------------------
    private int findBranchCode(Branch b)
    {
        SqlConnection con = null;
        int branchCode = 0;
        try
        {
            con = connect("DB7");
            String selectSTR = "select BranchCode from SM_Branch where Email='"+b.Email+"'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                branchCode = Convert.ToInt32(dr["BranchCode"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return branchCode;
    }

    
         private int findTrainerCode(Trainer t)
    {
        SqlConnection con = null;
        int trainerCode = 0;
        try
        {
            con = connect("DB7");
            String selectSTR = "select TrainerCode from SM_Trainer where Email='" + t.Email + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                trainerCode = Convert.ToInt32(dr["TrainerCode"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return trainerCode;
    }
    public Branch insertBranch(Branch b)
    {
        int numEffected = 0;
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand(b);      // helper method to build the insert string
       // SqlCommand scalarStr = new SqlCommand("select * from SM_Branch where Email = '" + branch.Email + "'");

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            numEffected = cmd.ExecuteNonQuery(); // execute the command
            //numEffected = Convert.ToInt32(scalarStr.ExecuteScalar());

        }
        catch (Exception ex)
        {
            return null;
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        b.BranchCode = findBranchCode(b);

        return b;
    }

    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertCommand(Branch branch)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', '{3}' ,'{4}', '{5}' ,'{6}', '{7}')", branch.Name, branch.Address, branch.PhoneNo, branch.Email, branch.Description, branch.CompanyNo.ToString(), branch.Password, branch.AreaCode.ToString());
        String prefix = "INSERT INTO SM_Branch" + "(BranchName , BranchAddress  , PhoneNo , Email ,BranchDescription,CompanyNo , BranchPassword ,AreaCode ) ";
        command = prefix + sb.ToString();

        return command;
    }
    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandTrainer(Trainer t)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', '{3}' ,'{4}', '{5}' ,'{6}', '{7}' ,{8},'{9}','{10}')", t.FirstName, t.LastName, t.Email, t.Phone1, t.Phone2, t.Gender, t.Password, t.AboutMe, t.PricePerHour.ToString(), t.DateOfBirth, t.Image);
        String prefix = "INSERT INTO SM_Trainer" + "(FirstName , LastName , Email , PhoneNo1 ,PhoneNo2 ,Gender ,TrainerPassword , AboutMe , MinPricePerHour , DateOfBirth, Photo  ) ";
        command = prefix + sb.ToString();

        return command;
    }
    public Trainer insertTrainer(Trainer trainer)
    {
        int numEffected = 0;
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandTrainer(trainer);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            numEffected = cmd.ExecuteNonQuery(); // execute the command

        }
        catch (Exception ex)
        {
            //return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        trainer.TrainerCode = findTrainerCode(trainer);
        return trainer;
    }

    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandTrainerQualification(TrainerQualification tQ)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1} ,{2}, '{3}', '{4}' )", tQ.TrainerCode.ToString(), tQ.QualificationTypeCode.ToString(), tQ.PopulationCode.ToString(), tQ.DocumentPath, tQ.FromDate);
        String prefix = "INSERT INTO SM_TrainerQualification" + "(TrainerCode , QualificationTypeCode ,PopulationCode , DocumentPath, FromDate ) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int insertTrainerQualification(TrainerQualification[] tQ)
    {

        SqlConnection con;
        SqlCommand cmd;
        int numEffected = 0;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        foreach (var item in tQ)
        {
            String cStr = BuildInsertCommandTrainerQualification(item);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                numEffected += cmd.ExecuteNonQuery(); // execute the command

            }
            catch (Exception ex)
            {
                con.Close();
                // write to log
                throw (ex);
            }
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return numEffected;
    }

    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandTrainerArea(TrainerArea area)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}' )", area.TrainerCode.ToString(), area.AreaCode.ToString());
        String prefix = "INSERT INTO SM_TrainerArea" + "(TrainerCode , AreaCode) ";
        command = prefix + sb.ToString();

        return command;
    }
    public int insertTrainerArea(TrainerArea[] area)
    {

        SqlConnection con;
        SqlCommand cmd;
        int numEffected = 0;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        foreach (var item in area)
        {
            String cStr = BuildInsertCommandTrainerArea(item);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                numEffected += cmd.ExecuteNonQuery(); // execute the command

            }
            catch (Exception ex)
            {
                con.Close();
                // write to log
                throw (ex);
            }
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return numEffected;
    }


    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandTrainerLanguage(TrainerLanguage lan)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}' )", lan.LCode.ToString(), lan.TrainerCode.ToString());
        String prefix = "INSERT INTO SM_LanguageTrainer" + "(LCode , TrainerCode) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int insertTrainerLanguage(TrainerLanguage[] lan)
    {

        SqlConnection con;
        SqlCommand cmd;
        int numEffected = 0;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        foreach (var item in lan)
        {
            String cStr = BuildInsertCommandTrainerLanguage(item);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                numEffected += cmd.ExecuteNonQuery(); // execute the command

            }
            catch (Exception ex)
            {
                con.Close();
                // write to log
                throw (ex);
            }
        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return numEffected;
    }

    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandRequest(RequestForReplacement r)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', '{3}' ,'{4}', '{5}' ,'{6}', '{7}' ,'{8}', '{9}' ,'{10}', '{11}' ,'{12}')", r.PublishDateTime, r.ContactName, r.BranchCode, r.ClassTypeCode, r.FromHour, r.ToHour, r.ReplacementDate, r.ClassDescription, r.Comments, r.DifficultyLevelCode, r.MaxPrice, r.LanguageCode, r.PopulationCode);
        String prefix = "INSERT INTO SM_RequestForReplacment" + "(PublishDateTime , ContactName , BranchCode , ClassTypeCode ,FromHour ,ToHour ,ReplacmentDate , ClassDecription ,Comments ,DifficultyLevelCode ,MaxPrice ,LanguageLCode ,PopulationCode )";
        command = prefix + sb.ToString();

        return command;
    }

    private int findRequestCode(RequestForReplacement r)
    {
        SqlConnection con = null;
        int ReplacmentCode = 0;
        try
        {
            con = connect("DB7");
            String selectSTR = "select max(ReplacmentCode) as ReplacmentCode from SM_RequestForReplacment where BranchCode='" + r.BranchCode + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                ReplacmentCode = Convert.ToInt32(dr["ReplacmentCode"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return ReplacmentCode;
    }

    public int insertRequest(RequestForReplacement r)
    {
       // int requestId;
        int numEffected = 0;
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandRequest(r);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
           // requestId = (Int32)cmd.ExecuteScalar();
           numEffected = cmd.ExecuteNonQuery(); // execute the command

        }
        catch (Exception ex)
        {
            //return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        r.ReplacementCode = findRequestCode(r);
        return r.ReplacementCode;
    }
    
    //static public int insertRequest(RequestForReplacement r, string connString)
    //{
    //    Int32 requestId = 0;
    //    string sql =
    //      "INSERT INTO SM_RequestForReplacment" + "(PublishDateTime, ContactName, BranchCode, ClassTypeCode, FromHour, ToHour, ReplacmentDate, ClassDecription, Comments, DifficultyLevelCode, MaxPrice, LanguageLCode, PopulationCode) Values(@r.PublishDateTime, r.ContactName, r.BranchCode, r.ClassTypeCode, r.FromHour, r.ToHour, r.ReplacementDate, r.ClassDescription, r.Comments, r.DifficultyLevelCode, r.MaxPrice, r.LanguageCode, r.PopulationCode)"
    //  + "SELECT CAST(scope_identity() AS int)";
    //    using (SqlConnection conn = new SqlConnection(connString))
    //    {
    //        SqlCommand cmd = new SqlCommand(sql, conn);
    //        cmd.Parameters.Add("@Name", SqlDbType.VarChar);
    //        cmd.Parameters["@name"].Value = newName;
    //        try
    //        {
    //            conn.Open();
    //            requestId = (Int32)cmd.ExecuteScalar();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //        }
    //    }
    //    return (int)requestId;
    //}

    private String BuildInsertCommandParameter(Parameter p)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}')", p.Pname);
        String prefix = "INSERT INTO SM_Parameters" + "(ParameterName) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int InsertParameter(Parameter parameter)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandParameter(parameter);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    private String BuildInsertCommandBranchParameter(BranchParameter bp)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}' , '{1}' , '{2}')", bp.BranchCode.ToString(), bp.ParameterCode.ToString(), bp.ParameterWeight.ToString());
        String prefix = "INSERT INTO SM_ParametersBrnach" + "(BranchCode , ParameterCode , ParameterWeight) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int InsertBranchParameter(BranchParameter Bparameter)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandBranchParameter(Bparameter);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    private String BuildUpdateCommandBranchParameter(BranchParameter b)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        string prefix = "UPDATE SM_ParametersBrnach Set ParameterWeight='" + b.ParameterWeight + "' WHERE BranchCode='" + b.BranchCode + "'"+ "AND ParameterCode = '" + b.ParameterCode + "'";
        command = prefix + sb.ToString();
        return command;
    }
    public int UpdateBranchParameter(BranchParameter [] b)
    {

        SqlConnection con;
        SqlCommand cmd;
        int numEffected = 0;
        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        foreach (var item in b)
        {
            String cStr = BuildUpdateCommandBranchParameter(item); // helper method to build the insert string
            cmd = CreateCommand(cStr, con);
            try
            {
                numEffected = cmd.ExecuteNonQuery(); // execute the command

            }
            catch (Exception ex)
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // write to log
                throw (ex);
            }
        }
        // create the command 
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return numEffected;
    }
   
    private String BuildInsertCommandLinksTo(LinksTo lt)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}' , '{1}' , '{2}')", lt.BranchCode.ToString(), lt.LinkName, lt.LinkCode.ToString());
        String prefix = "INSERT INTO SM_LinksTo" + "(BranchCode , Link , LinkCode) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int InsertLinksTo(LinksTo lt)
    {
        int numEffected = 0;
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DB7"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandLinksTo(lt);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            numEffected = cmd.ExecuteNonQuery(); // execute the command

        }
        catch (Exception ex)
        {
            //return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
        return numEffected;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }

    public List<Area> getAreas()
    {
        List<Area> areasList = new List<Area>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM SM_Area";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Area a = new Area();

                a.AreaCode = Convert.ToInt32(dr["AreaCode"]);
                a.AreaName = (string)dr["AreaName"];

                areasList.Add(a);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return areasList;
    }

    public List<Link> getLinks()
    {
        List<Link> linksList = new List<Link>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM SM_Links";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Link l = new Link();

                l.LinkCode = Convert.ToInt32(dr["LinkCode"]);
                l.LinkName = (string)dr["LinkName"];

                linksList.Add(l);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return linksList;
    }

    public List<Company> getCompany()
    {
        List<Company> companyList = new List<Company>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT CompanyNo, CompanyName FROM SM_Company";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Company c = new Company();

                c.CompanyNo = Convert.ToInt32(dr["CompanyNo"]);
                c.Name = (string)dr["CompanyName"];

                companyList.Add(c);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return companyList;
    }
    public List<User> getUsers(string email)
    {
        List<User> usersList = new List<User>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select distinct TrainerCode as Code, Email, TrainerPassword as 'Password', 'Trainer' as Type from SM_Trainer where Email = '"+email+ "' UNION select distinct BranchCode as Code, Email, BranchPassword as 'Password' , 'Branch' as Type from SM_Branch where Email = '" + email + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                User u = new User();

                u.Code = Convert.ToInt32(dr["Code"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.Type = (string)dr["Type"];

                usersList.Add(u);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return usersList;
    }

    public List<Qualification> getQualification()
    {
        List<Qualification> qualList = new List<Qualification>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select * from SM_Qualification";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Qualification q = new Qualification();

                q.TypeCode = Convert.ToInt32(dr["TypeCode"]);
                q.TypeName = (string)dr["TypeName"];
                //q.Description = (string)dr["QualificationDescription"];

                qualList.Add(q);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return qualList;
    }

    public List<Population> getPopulation()
    {
        List<Population> popList = new List<Population>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select * from SM_Population";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Population p = new Population();

                p.Code = Convert.ToInt32(dr["Code"]);
                p.PName = (string)dr["PName"];

                popList.Add(p);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return popList;
    }

    public List<Language> getLanguage()
    {
        List<Language> langList = new List<Language>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select * from SM_Language";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Language l = new Language();

                l.LanCode = Convert.ToInt32(dr["LCode"]);
                l.LanName = (string)dr["LName"];

                langList.Add(l);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return langList;
    }

    public List<DifficultyLevel> getDifficultyLevel()
    {
        List<DifficultyLevel> DLList = new List<DifficultyLevel>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select * from SM_DifficultyLevel";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                DifficultyLevel dl = new DifficultyLevel();

                dl.LevelCode = Convert.ToInt32(dr["LevelCode"]);
                dl.LevelName = (string)dr["LevelName"];

                DLList.Add(dl);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return DLList;
    }

    public DataTable GetParameter()
    {
        SqlConnection con = null;

        try
        {
            con = connect("DB7");
            da = new SqlDataAdapter("select * from SM_Parameters", con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dt = ds.Tables[0];

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return dt;
    }

    public List<Parameter> GetParameterBranchList()
    {
        List<Parameter> ParametersList = new List<Parameter>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM SM_Parameters";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Parameter a = new Parameter();

                a.Pcode = Convert.ToInt32(dr["ParameterCode"]);
                a.Pname = (string)dr["ParameterName"];

                ParametersList.Add(a);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return ParametersList;
    }
    public List<BranchParameter> GetBranchParameterList(int BranchCode)
    {
        List<BranchParameter> ParametersBranchList = new List<BranchParameter>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM SM_ParametersBrnach WHERE BranchCode='" + BranchCode + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                BranchParameter a = new BranchParameter();

                a.BranchCode = Convert.ToInt32(dr["BranchCode"]);
                a.ParameterCode = Convert.ToInt32(dr["ParameterCode"]);
                a.ParameterWeight = (float)Convert.ToDouble(dr["ParameterWeight"]);


                ParametersBranchList.Add(a);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return ParametersBranchList;
    }
    public List<Trainer> GetTrainerList()
    {
        List<Trainer> TrainerList = new List<Trainer>();
        SqlConnection con = null;

        try
        {
            con = connect("DB7"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM SM_Trainer";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Trainer t = new Trainer();
                t.TrainerCode =Convert.ToInt32(dr["TrainerCode"]);
                t.FirstName = (string)dr["FirstName"];
                t.LastName = (string)dr["LastName"];
                t.Email = (string)dr["Email"];
                t.Phone1 = (string)dr["PhoneNo1"];
                t.Phone2 = (string)dr["PhoneNo2"];
                t.Gender = (string)dr["Gender"];
                t.Password = (string)dr["TrainerPassword"];
                t.AboutMe = (string)dr["AboutMe"];
                t.PricePerHour = Convert.ToInt32(dr["MinPricePerHour"]);
                t.DateOfBirth = (string)dr["DateOfBirth"];
              //  t.Image = (string)dr["Photo"];



                TrainerList.Add(t);
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return TrainerList;
    }
}



