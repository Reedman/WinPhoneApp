using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace NyAppHelper.Data
{
    /// <summary>
    /// 处理App的IsolateStorageFile
    /// 在IsolatedStorage的文件均保存为Json格式
    /// </summary>
    public class IsolateStorageFileExecutor<T>:IStorageExecutiveAsync<T>
    {

        private static readonly string _isolateStorageFolder = AppSettings.IsolateStorageDataFolderPath;

        /// <summary>
        /// 获得保存在IsolateStorageFile的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> Get(string key)
        {
            try
            {
                string jsonStr = String.Empty;
                JsonParser parser = new JsonParser();
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string fileName = String.Concat(key, ".json");
                    string filePath = String.Concat(_isolateStorageFolder, @"/", fileName);
                    if (file.FileExists(filePath))
                    {
                        using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filePath, FileMode.Open, file))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                jsonStr = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                return parser.Deserialize<T>(jsonStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 向IsolateStorageFile的中添加对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Set(T target, string key)
        {
            bool result = false;
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string fileName = String.Concat(key, ".json");
                    string filePath = String.Concat(_isolateStorageFolder, @"/", fileName);
                    JsonParser parser = new JsonParser();
                    string jsonStr = parser.Serialize(target);
                    if (!file.DirectoryExists(_isolateStorageFolder))
                    {
                        file.CreateDirectory(_isolateStorageFolder);
                    }
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, file))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            await writer.WriteAsync(jsonStr);
                        }
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 判断在在IsolateStorageFile的是否存在键为key的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExists(string key)
        {
            bool isFileExists = false;
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string fileName = String.Concat(key, ".json");
                    string filePath = String.Concat(_isolateStorageFolder, @"/", fileName);
                    if (file.FileExists(filePath))
                    {
                        isFileExists = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return isFileExists;
        }
    }
}
