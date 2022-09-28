using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEditor;
using System.IO;
using MySql.Data.MySqlClient;

namespace Asset.MySql
{
    public enum ETableType
    {
        Account,
        Ranking,
        Max
    };

    public enum EAccountColumns
    {
        ID,
        Password,
        Email,
        Max
    }

    public enum ERankingColumns
    {
        ID,
        High_Record,
        Max
    }

    public class MySqlSetting
    {
        private static bool hasInit = false;

        private static string _connectionString;
        private static string[] _insertStrings = new string[(int) ETableType.Max];
        private static string _selectString;

        /// <summary>
        ///  MySql ���� �ʱ�ȭ
        /// </summary>
        public static void Init()
        {
            if(hasInit)
            {
                return;
            }

            Init(true);
        }
        /// <summary>
        /// MySql ������ �ʱ�ȭ
        /// </summary>
        /// <param name="isNeadReset"> �ʱ�ȭ�� �ʿ��ϸ� true, �ƴϸ� false</param>
        public static void Init(bool isNeadReset)
        {
            if(!isNeadReset)
            {
                return;
            }

            _connectionString = Resources.Load<TextAsset>("Connection").text;
            _insertStrings = Resources.Load<TextAsset>("Insert").text.Split('\n');
            _selectString = Resources.Load<TextAsset>("Select").text;

            SetEnum();
            Debug.Log("Enum Setting ��");
        }

        [MenuItem("Tools/GenerateEnum")]
        private static void SetEnum()
        {
            string settingString = Resources.Load<TextAsset>("SetEnum").text;
            string tableTypeString = settingString.Split('\n')[0];
            string columnTypeString = settingString.Split('\n')[1];

            List<string> tableNames = new List<string>();
            Dictionary<string, List<string>> columNames = new Dictionary<string, List<string>>();

            try
            {
                // DB���� ���̺�� �÷��� ��������
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    _sqlConnection.Open();

                    // ���̺� �� ��������
                    MySqlCommand tableTypeCommand = new MySqlCommand(tableTypeString, _sqlConnection);
                    MySqlDataReader tableTypeReader = tableTypeCommand.ExecuteReader();

                    while(true)
                    {
                        if(tableTypeReader.Read() == false)
                        {
                            break;
                        }

                        string tableName = tableTypeReader[0].ToString();
                        tableNames.Add(tableName);
                        columNames.Add(tableName, new List<string>());
                    }

                    tableTypeReader.Close();

                    // ���̺� �� ���� Column �� ��������
                    foreach(string table in tableNames)
                    {
                        string columnSelectString = columnTypeString + table + ";";
                        
                        MySqlCommand columnCommand = new MySqlCommand(columnSelectString, _sqlConnection);
                        MySqlDataReader columnTypeReader = columnCommand.ExecuteReader();

                        while(true)
                        {
                            if(columnTypeReader.Read() == false)
                            {
                                break;
                            }
                            
                            string columnName = columnTypeReader["Field"].ToString();
                            columNames[table].Add(columnName);
                        }
                        columnTypeReader.Close();
                    }

                    _sqlConnection.Close();
                }
            
                // �ش� ���뿡 �´� ���� �����ϱ�
                using (StreamWriter streamWriter = new StreamWriter("./Assets/Scripts/Util/MySqlEnum.cs"))
                {
                    // ��ó��
                    streamWriter.WriteLine("namespace Asset {");

                    // enum �����ϱ�
                    //  1. ���̺� Ÿ�� 
                    streamWriter.WriteLine("\tpublic enum ETableType {");
                    foreach(string table in tableNames)
                    {
                        streamWriter.WriteLine($"\t\t{table},");
                    }
                    streamWriter.WriteLine("\t}");

                    //  2. ���̺��� �÷� Ÿ��
                    foreach(string table in tableNames)
                    {
                        streamWriter.WriteLine($"\tpublic enum E{table}Columns {{");

                        foreach(string column in columNames[table])
                        {
                            streamWriter.WriteLine($"\t\t{column},");
                        }

                        streamWriter.WriteLine("\t}");
                    }

                    // ��ó��
                    streamWriter.WriteLine("}");
                }
                AssetDatabase.Refresh();
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return;
            }
        }

        /// <summary>
        /// ���� �߰��ϱ�
        /// </summary>
        /// <param name="ID">���� ID</param>
        /// <param name="Password">���� PW</param>
        /// <param name="Email">���� email</param>
        /// <returns>���������� �Է��� �Ǿ��� ��� true, �ƴϸ� false
        /// (��ǥ������ id�� email�� ��ĥ ��� false ��ȯ)</returns>
        public static bool AddNewAccount(string ID, string Password, string Email)
        {
            try
            {
                if(HasValue(EAccountColumns.ID, ID))
                {
                    throw new System.Exception("ID �ߺ���");
                }

                if(HasValue(EAccountColumns.Email, Email))
                {
                    throw new System.Exception("Email �ߺ���");
                }

                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertAccountString = GetInsertString(ETableType.Account, ID, Password, Email);
                    MySqlCommand _insertAccountCommand = new MySqlCommand(_insertAccountString, _mysqlConnection);

                    string _insertRankingString = GetInsertString(ETableType.Ranking, ID);
                    MySqlCommand _insertRankingCommand = new MySqlCommand(_insertRankingString, _mysqlConnection);

                    _mysqlConnection.Open();
                    _insertAccountCommand.ExecuteNonQuery();
                    _insertRankingCommand.ExecuteNonQuery();
                    _mysqlConnection.Close();
                }

                return true;
            } 
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }
        private static string GetInsertString(ETableType tableType, params string[] values)
        {
            string insertString = _insertStrings[(int)tableType] + '(';
            
            foreach(string value in values)
            {
                insertString += $"'{value}',";
            }
            
            insertString = insertString.TrimEnd(',') + ");";
            
            return insertString;
        }

        /// <summary>
        /// �ش� ���� DB�� �ִ��� Ȯ���Ѵ�.
        /// </summary>
        /// <param name="columnType">Account ���̺��� ���ϱ� ���� colum ��</param>
        /// <param name="value">���� ��</param>
        /// <returns>���� �ִٸ� true, �ƴϸ� false�� ��ȯ�Ѵ�.</returns>
        public static bool HasValue(EAccountColumns columnType, string value)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    bool result = false;

                    string selectString = _selectString + $" WHERE {columnType} = '{value}';";

                    _sqlConnection.Open();
                    
                    MySqlCommand _selectCommand = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader _selectData = _selectCommand.ExecuteReader();
                    
                    result = _selectData.Read();
                    
                    _sqlConnection.Close();

                    return result;
                }
            }
            catch
            {
                Debug.LogError( "������: Doublecheck");
                return false;
            }

        }

        /// <summary>
        /// Ranking Table���� baseType�� baseValue�� �������� checkType�� checkValue�� ��ġ�ϴ��� Ȯ����
        /// </summary>
        /// <param name="baseType">���� ������ Column Ÿ��</param>
        /// <param name="baseValue">���� ������ ��</param>
        /// <param name="checkType">Ȯ���� ������ Column Ÿ��</param>
        /// <param name="checkValue">Ȯ���� ��</param>
        /// <returns>��ġ�ϸ� true�� ��ȯ, �ƴϰų� ������ ���� ��� false ��ȯ</returns>
        public static bool CheckValueByBase(ERankingColumns baseType, string baseValue,
            ERankingColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.Ranking, baseType, baseValue, checkType, checkValue);
        }
        /// <summary>
        /// Account Table���� baseType�� baseValue�� �������� checkType�� checkValue�� ��ġ�ϴ��� Ȯ����
        /// ex. ID(baseType)�� aaa(baseValue)�� �������� Password(checkType)�� 123(checkValue)���� Ȯ����
        /// </summary>
        /// <param name="baseType">���� ������ Column Ÿ��</param>
        /// <param name="baseValue">���� ������ ��</param>
        /// <param name="checkType">Ȯ���� ������ Column Ÿ��</param>
        /// <param name="checkValue">Ȯ���� ��</param>
        /// <returns>��ġ�ϸ� true�� ��ȯ, �ƴϰų� ������ ���� ��� false ��ȯ</returns>
        public static bool CheckValueByBase(EAccountColumns baseType, string baseValue,
            EAccountColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.Account, baseType, baseValue, checkType, checkValue);
        }
        private static bool CheckValueByBase<T>(ETableType targetTable, T baseType, string baseValue,
            T checkType, string checkValue) where T : System.Enum
        {
            string checkTargetValue = GetValueByBase(targetTable, baseType, baseValue, checkType);
            if (checkTargetValue != null)
            {
                return checkTargetValue == checkValue;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Account ���̺��� baseType�� baseValue�� �������� targetType�� �����͸� ������
        /// </summary>
        /// <param name="baseType">������ �Ǵ� ���� Column��</param>
        /// <param name="baseValue">������ �Ǵ� ������</param>
        /// <param name="targetType">�������� ���� ������ Column��</param>
        /// <returns>�ش� �����͸� ��ȯ. ���� �� null ��ȯ</returns>
        public static string GetValueByBase(EAccountColumns baseType, string baseValue, EAccountColumns targetType)
        {
            return GetValueByBase(ETableType.Account, baseType, baseValue, targetType);
        }
        /// <summary>
        /// Ranking ���̺��� baseType�� baseValue�� �������� targetType�� �����͸� ������
        /// </summary>
        /// <param name="baseType">������ �Ǵ� ���� Column��</param>
        /// <param name="baseValue">������ �Ǵ� ������</param>
        /// <param name="targetType">�������� ���� ������ Column��</param>
        /// <returns>�ش� �����͸� ��ȯ. ���� �� null ��ȯ</returns>
        public static string GetValueByBase(ERankingColumns baseType, string baseValue, ERankingColumns targetType)
        {
            return GetValueByBase(ETableType.Ranking, baseType, baseValue, targetType);
        }
        private static string GetValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string selectString = $"Select {targetType} from {targetTable} where {baseType} = '{baseValue}';";

                    _sqlConnection.Open();

                    MySqlCommand command = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader resultReader = command.ExecuteReader();

                    if (!resultReader.Read())
                    {
                        throw new System.Exception("base ���� ����");
                    }
                    
                    string result = resultReader[targetType.ToString()].ToString();

                    _sqlConnection.Close();

                    return result;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return null;
            }

        }

        /// <summary>
        /// Ranking Table���� baseType�� baseValue�� �������� TargetType�� TargetValue�� ������
        /// </summary>
        /// <param name="baseType">���� ���� Column��</param>
        /// <param name="baseValue">���� ���� ������</param>
        /// <param name="targetType">������ ���� Column��</param>
        /// <param name="targetValue">������ ��</param>
        /// <returns>���������� ����Ǿ��ٸ� true, �ƴϸ� false�� ��ȯ</returns>
        public static bool UpdateValueByBase(ERankingColumns baseType, string baseValue,
            ERankingColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        /// <summary>
        /// Ranking Table���� baseType�� baseValue�� �������� TargetType�� TargetValue�� ������
        /// </summary>
        /// <param name="baseType">���� ���� Column��</param>
        /// <param name="baseValue">���� ���� ������</param>
        /// <param name="targetType">������ ���� Column��</param>
        /// <param name="targetValue">������ ��</param>
        /// <returns>���������� ����Ǿ��ٸ� true, �ƴϸ� false�� ��ȯ</returns>
        public static bool UpdateValueByBase(ERankingColumns baseType, string baseValue,
            ERankingColumns targetType, string targetValue)
        {
            return UpdateValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        /// <summary>
        /// Account Table���� baseType�� baseValue�� �������� TargetType�� TargetValue�� ������
        /// </summary>
        /// <param name="baseType">���� ���� Column��</param>
        /// <param name="baseValue">���� ���� ������</param>
        /// <param name="targetType">������ ���� Column��</param>
        /// <param name="targetValue">������ ��</param>
        /// <returns>���������� ����Ǿ��ٸ� true, �ƴϸ� false�� ��ȯ</returns>
        public static bool UpdateValueByBase(EAccountColumns baseType, string baseValue,
            EAccountColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        /// <summary>
        /// Ranking Table���� baseType�� baseValue�� �������� TargetType�� TargetValue�� ������
        /// </summary>
        /// <param name="baseType">���� ���� Column��</param>
        /// <param name="baseValue">���� ���� ������</param>
        /// <param name="targetType">������ ���� Column��</param>
        /// <param name="targetValue">������ ��</param>
        /// <returns>���������� ����Ǿ��ٸ� true, �ƴϸ� false�� ��ȯ</returns>
        public static bool UpdateValueByBase(EAccountColumns baseType, string baseValue,
            EAccountColumns targetType, string targetValue)
        {
            return UpdateValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        private static bool UpdateValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType, int targetValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} set {targetType} = {targetValue} where {baseType} = '{baseValue}';";
                    MySqlCommand command = new MySqlCommand(updateString, _sqlConnection);

                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
                    _sqlConnection.Close();

                    return true;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }

        }
        private static bool UpdateValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType, string targetValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} set {targetType} = '{targetValue}' where {baseType} = '{baseValue}';";
                    MySqlCommand command = new MySqlCommand(updateString, _sqlConnection);

                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
                    _sqlConnection.Close();

                    return true;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }

        }

        /// <summary>
        /// Account Table���� orderByType ���� ������������ limitCount��ŭ�� �����͸� �������ִ� �Լ�
        /// </summary>
        /// <param name="orderbyType">������ ����</param>
        /// <param name="limitCount">������ ������(row) ����</param>
        /// <param name="datas">������ ������ colum Ÿ�Ե�</param>
        /// <returns>�����͸� �Ѱ���. ������ �ִٸ� �� ����Ʈ�� ��ȯ�Ѵ�.</returns>
        public static List<Dictionary<string, string>> GetDataByOrderLimitRowCount(EAccountColumns orderbyType, 
            int limitCount, params EAccountColumns[] datas)
        {
            return GetDataByOrderLimitRowCount(ETableType.Account, orderbyType, limitCount, datas);
        }

        /// <summary>
        /// Ranking Table���� orderByType ���� ������������ limitCount��ŭ�� �����͸� �������ִ� �Լ�
        /// </summary>
        /// <param name="orderbyType">������ ����</param>
        /// <param name="limitCount">������ ������(row) ����</param>
        /// <param name="datas">������ ������ colum Ÿ�Ե�</param>
        /// <returns>�����͸� �Ѱ���. ������ �ִٸ� �� ����Ʈ�� ��ȯ�Ѵ�.</returns>
        public static List<Dictionary<string, string>> GetDataByOrderLimitRowCount(ERankingColumns orderbyType, 
            int limitCount, params ERankingColumns[] datas)
        {
            return GetDataByOrderLimitRowCount(ETableType.Ranking, orderbyType, limitCount, datas);
        }

        private static List<Dictionary<string, string>> GetDataByOrderLimitRowCount<T>(ETableType targetTable,
            T orderbyType, int limitCount, params T[] datas)
        {
            try
            {
                string selectString = "Select ";
                foreach(T data in datas)
                {
                    selectString += $"{data},";
                }
                selectString = selectString.TrimEnd(',') +
                    $" From {targetTable} Order By {orderbyType} DESC Limit {limitCount}";

                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    _sqlConnection.Open();
                    MySqlCommand command = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader dataReader = command.ExecuteReader();

                    for(int i = 0; i < limitCount; ++i)
                    {
                        result.Add(new Dictionary<string, string>());
                        if(dataReader.Read())
                        {
                            foreach(T data in datas)
                            {
                                result[i][data.ToString()] = dataReader[data.ToString()].ToString();
                            }
                        }
                        else
                        {
                            foreach (T data in datas)
                            {
                                result[i][data.ToString()] = "";
                            }
                        }
                    }

                    _sqlConnection.Close();
                }

                return result;
            }
            catch(System.Exception error)
            {
                Debug.LogError(error.Message);
                return new List<Dictionary<string, string>>();
            }
        }
    }
}
