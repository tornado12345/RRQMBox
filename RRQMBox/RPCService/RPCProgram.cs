//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using System;

namespace Demo.Service
{
    internal class RPCProgram
    {
        private static void Main(string[] args)
        {
            RPCService rpcService = new RPCService();
            rpcService.RegistAllService();

            TcpRPCParser tcpRPCParser = new TcpRPCParser();
            tcpRPCParser.SerializeConverter = new BinarySerializeConverter();
            tcpRPCParser.Bind(7789, 2);
            tcpRPCParser.NameSpace = "RRQMTest";
            Console.WriteLine("TCP解析器添加完成");

            UdpRPCParser udpRPCParser = new UdpRPCParser();
            udpRPCParser.SerializeConverter = new BinarySerializeConverter();
            udpRPCParser.NameSpace = "RRQMTest";
            udpRPCParser.Bind(7790, 2);
            Console.WriteLine("UDP解析器添加完成");

            TcpRPCParser tcpXmlRPCParser = new TcpRPCParser();
            tcpXmlRPCParser.SerializeConverter = new XmlSerializeConverter();
            tcpXmlRPCParser.NameSpace = "RRQMTest";
            tcpXmlRPCParser.Bind(7791, 2);
            Console.WriteLine("TCPXml解析器添加完成");

            WebApiParser webApiParser = new WebApiParser();
            webApiParser.Bind(7792, 2);
            Console.WriteLine("webApiParser解析器添加完成");

            rpcService.AddRPCParser("TcpParser", tcpRPCParser);
            rpcService.AddRPCParser("UdpParser", udpRPCParser);
            rpcService.AddRPCParser("tcpXmlRPCParser", tcpXmlRPCParser);
            rpcService.AddRPCParser("webApiParser", webApiParser);

            rpcService.OpenRPCServer();
            Console.WriteLine("RPC启动完成");
            Console.ReadKey();
        }
    }
}