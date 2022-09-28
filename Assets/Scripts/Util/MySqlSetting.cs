using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using MySql.Data.MySqlClient;

namespace MySql
{
    public class MySqlSetting
    {
        public enum ETableType
        {
            Account,
            Ranking,
            Max
        };

        public enum EAccountColumnType
        {
            ID,
            Password,
            Email,
            Max
        }

        public enum ERankingColumType
        {
            ID,
            High_Record,
            Max
        }

        private static bool hasInit = false;

        private static string _connectionString;
        private static string[] _insertStrings = new string[(int) ETableType.Max];
        private static string _selectString;

        /// <summary>
        ///  MySql 세팅 초기화
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
        /// MySql 세팅을 초기화
        /// </summary>
        /// <param name="isNeadReset"> 초기화가 필요하면 true, 아니면 false</param>
        public static void Init(bool isNeadReset)
        {
            if(!isNeadReset)
            {
                return;
            }

            _connectionString = Resources.Load<TextAsset>("Connection").text;
            _insertStrings = Resources.Load<TextAsset>("Insert").text.Split('\n');
            _selectString = Resources.Load<TextAsset>("Select").text;
        }

        /// <summary>
        /// 계정 추가하기
        /// </summary>
        /// <param name="ID">계정 ID</param>
        /// <param name="Password">계정 PW</param>
        /// <param name="Email">계정 email</param>
        /// <returns>정상적으로 입력이 되었을 경우 true, 아니면 false
        /// (대표적으로 id나 email이 겹칠 경우 false 반환)</returns>
        public static bool AddNewAccount(string ID, string Password, string Email)
        {
            try
            {
                if(IsThereValue(EAccountColumnType.ID, ID))
                {
                    throw new System.Exception("ID 중복됨");
                }

                if(IsThereValue(EAccountColumnType.Email, Email))
                {
                    throw new System.Exception("Email 중복됨");
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
            
            Debug.Log(insertString);
            return insertString;
        }

        /// <summary>
        /// 해당 값이 DB에 있는지 확인한다.
        /// </summary>
        /// <param name="columnType">Account 태이블에서 비교하기 위한 colum 명</param>
        /// <param name="value">비교할 값</param>
        /// <returns>값이 있다면 true, 아니면 false를 반환한다.</returns>
        public static bool IsThereValue(EAccountColumnType columnType, string value)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    bool result = false;

                    string selectString = _selectString + $" WHERE {columnType} = '{value}';";
                    Debug.Log(selectString);

                    _sqlConnection.Open();
                    
                    MySqlCommand _selectCommand = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader _selectData = _selectCommand.ExecuteReader();
                    
                    result = _selectData.Read();
                    Debug.Log(result);
                    
                    _sqlConnection.Close();

                    return result;
                }
            }
            catch
            {
                Debug.LogError( "오류남: Doublecheck");
                return false;
            }

        }

        /// <summary>
        /// TargetTable에서 base 값을 기준으로 check 값이 맞는지 확인하는 함수
        /// ex. base (ID) 값을 기준으로 check (Password) 값이 맞는지 확인
        /// </summary>
        /// <typeparam name="T">Column 종류(AccountColumn이나 RankingColumn 등)</typeparam>
        /// <param name="targetTable">값을 검색할 table</param>
        /// <param name="baseType">기준 Column</param>
        /// <param name="baseValue">기준 값</param>
        /// <param name="checkType">확인할 Column</param>
        /// <param name="checkValue">확인할 값</param>
        /// <returns>값이 맞으면 true, 아니거나 오류가 있다면 false</returns>
        public static bool CheckValueByBase(ERankingColumType baseType, string baseValue,
            ERankingColumType checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.Ranking, baseType, baseValue, checkType, checkValue);
        }
        public static bool CheckValueByBase(EAccountColumnType baseType, string baseValue,
            EAccountColumnType checkType, string checkValue)
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

        public static string GetValueByBase(EAccountColumnType baseType, string baseValue, EAccountColumnType targetType)
        {
            return GetValueByBase(ETableType.Account, baseType, baseValue, targetType);
        }
        public static string GetValueByBase(ERankingColumType baseType, string baseValue, ERankingColumType targetType)
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
                        throw new System.Exception("base 값이 없음");
                    }
                    string result = resultReader[targetType.ToString()].ToString();
                    Debug.Log(result);

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

        public static bool SetValueByBase(ERankingColumType baseType, string baseValue,
            ERankingColumType targetType, int targetValue)
        {
            return SetValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        public static bool SetValueByBase(ERankingColumType baseType, string baseValue,
            ERankingColumType targetType, string targetValue)
        {
            return SetValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        public static bool SetValueByBase(EAccountColumnType baseType, string baseValue,
            EAccountColumnType targetType, int targetValue)
        {
            return SetValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        public static bool SetValueByBase(EAccountColumnType baseType, string baseValue,
            EAccountColumnType targetType, string targetValue)
        {
            return SetValueByBase(ETableType.Ranking, baseType, baseValue, targetType, targetValue);
        }
        private static bool SetValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType, int targetValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} set {targetType} = {targetValue} where {baseType} = '{baseValue}';";
                    Debug.Log(updateString);
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

        private static bool SetValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType, string targetValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} set {targetType} = '{targetValue}' where {baseType} = '{baseValue}';";
                    Debug.Log(updateString);
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
        /// Account Table에서 orderByType 기준 내림차순으로 limitCount만큼의 데이터를 가져와주는 함수
        /// </summary>
        /// <param name="orderbyType">순서의 기준</param>
        /// <param name="limitCount">가져올 데이터(row) 갯수</param>
        /// <param name="datas">가져올 데이터 colum 타입들</param>
        /// <returns>데이터를 넘겨줌. 오류가 있다면 빈 리스트를 반환한다.</returns>
        public static List<Dictionary<string, string>> GetDataByOrderLimitN(EAccountColumnType orderbyType, 
            int limitCount, params EAccountColumnType[] datas)
        {
            return GetDataByOrderLimitN(ETableType.Account, orderbyType, limitCount, datas);
        }

        /// <summary>
        /// Ranking Table에서 orderByType 기준 내림차순으로 limitCount만큼의 데이터를 가져와주는 함수
        /// </summary>
        /// <param name="orderbyType">순서의 기준</param>
        /// <param name="limitCount">가져올 데이터(row) 갯수</param>
        /// <param name="datas">가져올 데이터 colum 타입들</param>
        /// <returns>데이터를 넘겨줌. 오류가 있다면 빈 리스트를 반환한다.</returns>
        public static List<Dictionary<string, string>> GetDataByOrderLimitN(ERankingColumType orderbyType, 
            int limitCount, params ERankingColumType[] datas)
        {
            return GetDataByOrderLimitN(ETableType.Ranking, orderbyType, limitCount, datas);
        }

        private static List<Dictionary<string, string>> GetDataByOrderLimitN<T>(ETableType targetTable,
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
                Debug.Log(selectString);

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
