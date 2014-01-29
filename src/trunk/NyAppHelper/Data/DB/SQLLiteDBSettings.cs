using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Data.Linq;
using System.Data.Linq;
using NyAppHelper.Model;

namespace NyAppHelper.Data
{
    /// <summary>
    /// APP全局的Data Context对象
    /// </summary>
    public class AppDBDataContext : DataContext
    {
        public AppDBDataContext(string connStr) : base(connStr) { }

        /// <summary>
        /// 定义一个农田表
        /// </summary>
        public Table<Field> FieldTable;

        /// <summary>
        /// 定义一个种植作物表
        /// </summary>
        public Table<Crop> CropTable;

        /// <summary>
        /// 定义一个行政区域表
        /// </summary>
        public Table<Region> RegionTable;

        /// <summary>
        /// 定义了一个农田作物表
        /// </summary>
        public Table<FieldCrop> FieldCropTable;

        /// <summary>
        /// 定义一个采集信息任务表
        /// </summary>
        public Table<CollectionTask> CollectionTaskTable;

        /// <summary>
        /// 定义了一下采集任务自然环境要求表
        /// </summary>
        public Table<CollectionTaskNature> CollectionTaskNatureTable;

        /// 定义了一下采集任务病虫害要求表
        /// </summary>
        public Table<CollectionTaskPestView> CollectionPestViewTable;

        /// 定义了一下病虫草海数据字典信息表
        /// </summary>
        public Table<Pest> PestTable;

        /// <summary>
        /// 定义一个采集结果表
        /// </summary>
        public Table<CollectionTaskResult> CollectionTaskResultTable;

        /// <summary>
        /// 定义了一个采集结果病害表
        /// </summary>
        public Table<CollectionTaskResultDisease> CollectionTaskResultDiseaseTable;

        /// <summary>
        /// 定义了一个采集结果虫害表
        /// </summary>
        public Table<CollectionTaskResultPest> CollectionTaskResultPestTable;

        /// <summary>
        /// 定义了一个采集结果草害表
        /// </summary>
        public Table<CollectionTaskResultWeed> CollectionTaskResultWeedTable;

        /// <summary>
        /// 定义了一个照片表
        /// </summary>
        public Table<Photo> PhotoTable;

    }

    public class SQLLiteDBSettings
    {

        /// <summary>
        ///WP APP的SQLLite数据库
        ///初始化类，如果数据库不存在，就创建新的数据库
        ///如果存在，检查数据库的版本号，如果过期，更新数据库结构
        /// </summary>
        public static void InitAppDBSettings()
        {
            using (var dbContext = new AppDBDataContext(AppSettings.DBConStr))
            {
                try
                {
                    if (!dbContext.DatabaseExists())
                    {
                        dbContext.CreateDatabase();

                        //创建手机的数据库版本
                        DatabaseSchemaUpdater dbUpdater = dbContext.CreateDatabaseSchemaUpdater();
                        dbUpdater.DatabaseSchemaVersion = AppSettings.AppDBVersion;
                        dbUpdater.Execute();
                    }
                    else
                    {
                        DatabaseSchemaUpdater dbUpdater = dbContext.CreateDatabaseSchemaUpdater();
                        if (dbUpdater.DatabaseSchemaVersion < AppSettings.AppDBVersion)
                        {
                            //删除旧版本的数据库
                            dbContext.DeleteDatabase();

                            //更新数据库结构
                            dbContext.CreateDatabase();

                            //创建手机的数据库版本
                            dbUpdater.DatabaseSchemaVersion = AppSettings.AppDBVersion;
                            dbUpdater.Execute();
                        }
                    }
                }
                catch (Exception)
                {
                    //删除旧版本的数据库
                    dbContext.DeleteDatabase();

                    //更新数据库结构
                    dbContext.CreateDatabase();
                }
            }
        }



    }
}
