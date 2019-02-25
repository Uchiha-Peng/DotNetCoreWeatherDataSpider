# DotNetCoreWeatherDataSpider

#### 项目说明：

**该程序基于.NET Core 2.2+HtmlAgilityPack从http://www.tianqihoubao.com/抓取指定城市从上一年1月1日到现在为止的历史天气数据。并将数据通过控制台打印出来，或是存储到SQLite数据库中**

#### 项目运行：

请确保本地已安装.NET Core 2.2环境，cd进入项目根目录后，请先restore项目，再使用DotNet命令生成数据库：

```
dotnet resore

dotnet ef migrations add InitialCreate
 
dotnet ef database update
```

> **若使用VS Code或PowerShell打开项目，则可直接使用`dotnet run`运行**
>
> **若选择Visual Studio打开项目**，请右键“项目”》“属性”》“调试”》“工作目录”，将项目根目录设置为工作目录，或直接将`SQLiteDB.cs`文件中的SQLite连接字符串相对路径`"Data Source=Weather.db"`绝对路径`"Data Source=C:/Users/Amaterasu/source/repos/CwTestApp/CwTestApp/Weather.db"`，否则运行的时候会报错“No Such Table:xxxx”，这是因为VS的项目默认的工作目录是bin/debug目录，详细见：https://github.com/aspnet/EntityFramework.Docs/issues/735