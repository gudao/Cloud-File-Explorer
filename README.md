NetHadoop使用说明
=========

C#写的Winform客户端，可以用来连接Hadoop集群，管理Hadoop文件。
  本项目主要为了解决Windows平台下的访问Hadoop文件的问题。使用Hadoop文件系统的朋友应该都知道Hadoop是Java语言开发的，
并且需要部署在Linux平台上，所以这就对于.Net平台的开发者带来诸多困难。本项目以C#语言通过thrift方式连接Hadoop的HDFS
提供Hadoop文件的读、写、删除、重命名、列表等功能，同时提供一个WinForm形式的资源管理器，可以方面Windows平台下的用户
集中管理文件（Hadoop本身提供了一个Web形式的文件浏览接口，但是不能满足现有文件管理需求）。
 项目现有提供的功能包括：
  1、列表。可以以资源管理器方式列表文件及文件信息（如：文件格式、大小等）
  2、文件下载。可以将HDFS中的文件下载到本地，可以单个或批量，按照列表下载。
  3、文件上传。可以将本地的文件上传至HDFS，同样支持单个、批量。
  4、文件修改。可以直接重命名、复制、粘贴、剪切HDFS中的文件。
  5、删除文件恢复。可以自定义文件删除恢复机制，在指定的时间段内恢复文件。

部署说明：
  必备条件：
  1）Hadoop集群；
  2）Hadoop配置Thirft的Java端
  配置说明：
  打开NetHadoop.exe.config文件，按照下面示例配置修改为相应的服务地址
  <appSettings>
        <!--Thrift服务IP和端口-->
        <add key="HdfsThriftIP" value="192.168.3.10"/><!--Thrift服务的IP地址一般为Hadoop主节点IP-->
        <add key="HdfsThriftPort" value="8888"/><!--Thrift服务的端口，可以任意指定该机器的非使用端口-->
        <!--服务器根路径-->
        <add key="HdfsRoot" value="hdfs://192.168.3.10:9000/user/root"/><!--Hadoop文件目录的根目录-->
        <!--文件存储份数-->
        <add key="HDFSREPLICATION" value="2"/>
  </appSettings>
  
