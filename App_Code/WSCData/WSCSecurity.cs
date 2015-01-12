using System;
using System.Configuration;
using MDAAB.Classic;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;

namespace WSCData {
    /// <summary>
    /// Summary description for WSCSecurity.
    /// </summary>
    [Serializable()]
    public class WSCSecurity : ISerializable {

        private const string MOD_NAME = "WSCData.WSCSecurity.";

        private const string AG_ADMIN = "WSC AG ADMIN";
        private const string AG_AGRICULTURIST = "WSC AG AGRICULTURIST";
        private const string AG_BEET_CLERK = "WSC AG BEET CLERK";
//        private const string AG_MANAGER = "WSC AG MANAGER";
        private const string AG_READ_ONLY = "WSC AG READ-ONLY";
        private const string AG_SRS_ADMIN = "WSC AG SRS ADMIN";
        private const string AG_SRS_MANAGER = "WSC AG SRS MANAGER";
        private const string GUEST = "Guest";

        private string LF = System.Environment.NewLine;

        string _default = "";
        string _sessionID = "";
        int _userID = 0;
        string _userName = "";
        int _securityGroupID = 0;
        string _securityGroupName = null;
        ShsPermission _permission = ShsPermission.shsUndetermined;

        public enum ShsPermission {
            shsUndetermined,
            shsNone,
            shsReadOnly,
            shsReadWrite,
            shsAdmin
        }

        //Deserialization constructor.
        public WSCSecurity(SerializationInfo info, StreamingContext ctxt) {

            _default = (string)info.GetValue("default", typeof(string));
            _sessionID = (string)info.GetValue("sessionID", typeof(string));
            _userID = (int)info.GetValue("userID", typeof(int));
            _userName = (string)info.GetValue("userName", typeof(string));
            _securityGroupID = (int)info.GetValue("securityGroupID", typeof(int));
            _securityGroupName = (string)info.GetValue("securityGroupName", typeof(string));
            _permission = CInt2Permission((int)info.GetValue("permission", typeof(int)));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {

            info.AddValue("default", _default);
            info.AddValue("sessionID", _sessionID.ToString());
            info.AddValue("userID", _userID.ToString());
            info.AddValue("userName", _userName);
            info.AddValue("securityGroupID", _securityGroupID.ToString());
            info.AddValue("securityGroupName", _securityGroupName);
            info.AddValue("permission", Convert.ToInt32(_permission).ToString());
        }

        private void GetInfo(string userName, ref int userID,
            ref int securityGroupID, ref string securityGroupName) {

            string connStr = "";

            try {

                connStr = ConfigurationManager.ConnectionStrings["BeetConn"].ToString();
                using (SqlConnection conn = new SqlConnection(connStr)) {

                    string procName = "bawpEmpGetInfo";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = userName;
                    SetTimeout();

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);

                    if (spParams[1].Value == System.DBNull.Value) {
                        userID = 0;
                    } else {
                        userID = Convert.ToInt32(spParams[1].Value);
                    }
                    if (spParams[2].Value == System.DBNull.Value) {
                        securityGroupID = 0;
                    } else {
                        securityGroupID = Convert.ToInt32(spParams[2].Value);
                    }
                    if (spParams[3].Value == System.DBNull.Value) {
                        securityGroupName = "";
                    } else {
                        securityGroupName = spParams[3].Value.ToString();
                    }
                }
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    string errMsg = MOD_NAME + "GetInfo" + LF +
                        "userName: " + userName + LF +
                        "ConnStr: " + connStr.Substring(1, 30);
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + "GetInfo" + LF +
                    "userName: " + userName + LF +
                    "ConnStr: " + connStr.Substring(1, 30);
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public string SessionID {
            get { return _sessionID; }
            set { _sessionID = value; }
        }

        public string SecurityGroupName {
            get {
                return _securityGroupName;
            }
            set { _securityGroupName = value; }
        }

        public ShsPermission Permission {
            get { return _permission; }
            set { _permission = value; }
        }

        public WSCSecurity() {

            int userID = 0;
            int securityGroupID = 0;

            string empName = WSCIEMP.Common.AppHelper.GetIdentityName();
            int idxDomain = empName.LastIndexOf(@"\");
            if (idxDomain > 0) {
                empName = empName.Substring(idxDomain + 1);
            }
            string userName = empName;

            string securityGroupName = null;
            GetInfo(userName, ref userID, ref securityGroupID, ref securityGroupName);

            _userID = userID;
            _userName = userName;
            _securityGroupID = securityGroupID;
            _securityGroupName = securityGroupName;

            if (_securityGroupName.ToUpper() == AG_ADMIN ||
                _securityGroupName.ToUpper() == AG_SRS_ADMIN ||
                _securityGroupName.ToUpper() == AG_SRS_MANAGER) {

                _permission = ShsPermission.shsReadWrite;
            } else {
                if (_securityGroupName.ToUpper() == AG_READ_ONLY) {
                    _permission = ShsPermission.shsReadOnly;
                }
            }
        }

        public int UserID {

            get {
                return _userID;
            }
            set {
                _userID = value;
            }
        }

        public string UserName {
            get { return _userName; }
        }

        public static string Identity {
            get {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                return identity.Name;
            }
        }

        public ShsPermission AuthorizeShid(int shid, int cropYear) {

            const string METHOD_NAME = "AuthorizeShid";
            try {

                if (_permission == ShsPermission.shsUndetermined) {

                    // this will perform authorization against the shareholder's home factory
                    switch (_securityGroupName.ToUpper()) {

                        // WSC AG Agriculturist
                        case AG_AGRICULTURIST:
                        case AG_BEET_CLERK:
                            if (WSCEmployee.IsMemberAgriculturist(_userID, shid, cropYear)) {
                                return ShsPermission.shsReadWrite;
                            } else {
                                return ShsPermission.shsNone;
                            }

                        default:
                            return ShsPermission.shsNone;
                    }

                } else {
                    return _permission;
                }
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + "; shid: " + shid.ToString() + "; cropYear: " + cropYear.ToString(), ex);
                throw (wscEx);
            }

        }

        private static void SetTimeout() {
            int timeOut = Convert.ToInt32(ConfigurationManager.AppSettings["sql.command.timeout"].ToString());
            SqlHelper.CommandTimeout = timeOut.ToString();
        }

        private ShsPermission CInt2Permission(int perm) {

            ShsPermission p = ShsPermission.shsUndetermined;

            switch (perm) {
                case 0:
                    p = ShsPermission.shsUndetermined;
                    break;
                case 1:
                    p = ShsPermission.shsNone;
                    break;
                case 2:
                    p = ShsPermission.shsReadOnly;
                    break;
                case 3:
                    p = ShsPermission.shsReadWrite;
                    break;
                case 4:
                    p = ShsPermission.shsAdmin;
                    break;
            }
            return p;
        }

        public override String ToString() {

            string s = "UserID: " + _userID.ToString() + "; " +
                "UserName: " + _userName + "; " +
                "default: " + _default + "; " +
                "sessionID: " + _sessionID + "; " +
                "SecurityGroupID: " + _securityGroupID.ToString() + "; " +
                "SecurityGroupName: " + _securityGroupName + "; " +
                "ShsPermission: " + Convert.ToInt32(_permission).ToString();

            return s;
        }

        public static SqlDataReader SecurityRegionGetAll(SqlConnection conn) {

            const string METHOD_NAME = MOD_NAME + "SecurityRegionGetAll.";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRegionGetAll";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName);
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader SecurityGetUserRoles(SqlConnection conn, int userID) {

            const string METHOD_NAME = MOD_NAME + "SecurityGetUserRoles: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserSecurityGetUserRoles";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = userID;

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader SecurityGetUserRegions(SqlConnection conn, int userID) {

            const string METHOD_NAME = MOD_NAME + "SecurityGetUserRegions: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserSecurityGetUserRegions";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = userID;

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader SecurityRoleGetAll(SqlConnection conn) {

            const string METHOD_NAME = MOD_NAME + "SecurityRoleGetAll: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserSecurityRoles";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                //SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName);
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader SecurityRoleGetUsers(SqlConnection conn, int roleID) {

            const string METHOD_NAME = MOD_NAME + "SecurityRoleGetUsers: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserSecurityGetByRole";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = roleID;
                //SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader SecurityRoleGetUsers(SqlConnection conn, string loginName,
            int roleID, bool inActive) {

            const string METHOD_NAME = MOD_NAME + "SecurityRoleGetUsers: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserSecurityGetByLoginRole";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = loginName;
                spParams[1].Value = roleID;
                spParams[2].Value = (inActive ? 1 : 0);

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader RegionFactoryGetByRegion(SqlConnection conn, int regionID) {

            const string METHOD_NAME = MOD_NAME + "RegionFactoryGetByRegion: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRegionFactoryGetByRegion";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = regionID;

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return dr;
        }

        public static void UserSecurityRegionFactorySave(int regionID, string factories) {

            const string METHOD_NAME = MOD_NAME + "UserSecurityRegionFactorySave: ";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpRegionFactorySave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = regionID;
                    spParams[1].Value = factories;

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                throw (sqlEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void UserSecuritySave(int userID, bool isActive, string roles, string regions) {

            const string METHOD_NAME = MOD_NAME + "UserSecuritySave: ";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserSecuritySave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = userID;
                    spParams[1].Value = (isActive? 1: 0);
                    spParams[2].Value = roles;
                    spParams[3].Value = regions;

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                throw (sqlEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }
    }
}
