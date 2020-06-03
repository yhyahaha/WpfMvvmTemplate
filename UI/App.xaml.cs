using DataAccess;
using InterFaces;
using Model;
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using ViewModel;

namespace UI
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        IDbConnection dbConnection;
        const string provider = "Microsoft.ACE.OLEDB.12.0";
        const string dataSourceUri = @"PersonInfo.accdb";
        const string password = "yhyahaha";

        IDataAccess<Person> repository;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CreateDbConnection();

            //　Dependency Injection
            repository = new PersonRepository(dbConnection);
            IObjectMapping mapper = new MapperAutoMapper();
            IObjectMapping reverseMapper = new ReverseMapperAutoMapper();
            IOperation viewModel = new MainWindowViewModel(repository, mapper,reverseMapper);
            MainWindow window = new MainWindow(viewModel);

            // Close処理
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;

            window.Show();
        }

        private void CreateDbConnection()
        {
            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Provider = provider;
            builder.DataSource = dataSourceUri;
            builder["Jet OLEDB:Database Password"] = password;

            dbConnection = new OleDbConnection(builder.ConnectionString);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            dbConnection.Dispose();
        }
    }
}
