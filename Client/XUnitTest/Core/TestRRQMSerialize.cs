//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore;
using RRQMCore.ByteManager;
using RRQMCore.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTest.Core
{
    public class TestRRQMSerialize
    {
        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData(sbyte.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData(ulong.MinValue)]
        [InlineData("RRQM")]
        [InlineData('R')]
        public void ShouldSerializePrimitiveObjBeOk(object obj)
        {
            byte[] data = SerializeConvert.RRQMBinarySerialize(obj);

            object sobj = SerializeConvert.RRQMBinaryDeserialize(data, 0, obj.GetType());

            Assert.Equal(obj, sobj);
            Assert.Equal(obj.GetType(), sobj.GetType());
        }

        [Fact]
        public void ShouldSerializeObjBeOk()
        {
            Student student = new Student();
            student.P1 = 10;
            student.P2 = "若汝棋茗";
            student.P3 = 100;
            student.P4 = 0;
            student.P5 = DateTime.Now;
            student.P6 = 10;
            student.P7 = new byte[1024 * 64];
            student.P8 = new string[] { "I", "love", "you" };

            Random random = new Random();
            random.NextBytes(student.P7);

            student.List1 = new List<int>();
            student.List1.Add(1);
            student.List1.Add(2);
            student.List1.Add(3);

            student.List2 = new List<string>();
            student.List2.Add("1");
            student.List2.Add("2");
            student.List2.Add("3");

            student.List3 = new List<byte[]>();
            student.List3.Add(new byte[1024]);
            student.List3.Add(new byte[1024]);
            student.List3.Add(new byte[1024]);

            student.Dic1 = new Dictionary<int, int>();
            student.Dic1.Add(1, 1);
            student.Dic1.Add(2, 2);
            student.Dic1.Add(3, 3);

            student.Dic2 = new Dictionary<int, string>();
            student.Dic2.Add(1, "1");
            student.Dic2.Add(2, "2");
            student.Dic2.Add(3, "3");

            student.Dic3 = new Dictionary<string, string>();
            student.Dic3.Add("1", "1");
            student.Dic3.Add("2", "2");
            student.Dic3.Add("3", "3");

            student.Dic4 = new Dictionary<int, Arg>();
            student.Dic4.Add(1, new Arg(1));
            student.Dic4.Add(2, new Arg(2));
            student.Dic4.Add(3, new Arg(3));

            ByteBlock byteBlock = new ByteBlock(1024 * 512);
            SerializeConvert.RRQMBinarySerialize(byteBlock, student);
            Student newStudent = SerializeConvert.RRQMBinaryDeserialize<Student>(byteBlock.Buffer, 0);
            byteBlock.Dispose();

            Assert.Equal(student.P1, newStudent.P1);
            Assert.Equal(student.P2, newStudent.P2);
            Assert.Equal(student.P3, newStudent.P3);
            Assert.Equal(student.P4, newStudent.P4);
            Assert.Equal(student.P5, newStudent.P5);
            Assert.Equal(student.P6, newStudent.P6);

            Assert.NotNull(newStudent.P7);
            Assert.Equal(student.P7.Length, newStudent.P7.Length);

            Assert.NotNull(newStudent.P8);
            Assert.Equal(student.P8.Length, newStudent.P8.Length);

            for (int i = 0; i < student.P8.Length; i++)
            {
                Assert.Equal(student.P8[i], newStudent.P8[i]);
            }

            for (int i = 0; i < student.P7.Length; i++)
            {
                Assert.Equal(student.P7[i], newStudent.P7[i]);
            }

            Assert.NotNull(newStudent.List1);
            Assert.Equal(student.List1.Count, newStudent.List1.Count);
            for (int i = 0; i < student.List1.Count; i++)
            {
                Assert.Equal(student.List1[i], newStudent.List1[i]);
            }

            Assert.NotNull(newStudent.List2);
            Assert.Equal(student.List2.Count, newStudent.List2.Count);
            for (int i = 0; i < student.List2.Count; i++)
            {
                Assert.Equal(student.List2[i], newStudent.List2[i]);
            }

            Assert.NotNull(newStudent.List3);
            Assert.Equal(student.List3.Count, newStudent.List3.Count);
            for (int i = 0; i < student.List3.Count; i++)
            {
                Assert.Equal(student.List3[i].Length, newStudent.List3[i].Length);
            }

            Assert.NotNull(newStudent.Dic1);
            Assert.Equal(student.Dic1.Count, newStudent.Dic1.Count);

            Assert.NotNull(newStudent.Dic2);
            Assert.Equal(student.Dic2.Count, newStudent.Dic2.Count);

            Assert.NotNull(newStudent.Dic3);
            Assert.Equal(student.Dic3.Count, newStudent.Dic3.Count);

            Assert.NotNull(newStudent.Dic4);
            Assert.Equal(student.Dic4.Count, newStudent.Dic4.Count);
        }

        [Fact]
        public void ShouldSerializeMetadataBeOk()
        {
            Metadata metadata = new Metadata();//传递到服务器的元数据
            metadata.Add("1", "1");
            metadata.Add("2", "2");

            ByteBlock byteBlock = new ByteBlock(1024 * 512);
            SerializeConvert.RRQMBinarySerialize(byteBlock, metadata);
            Metadata newMetadata = SerializeConvert.RRQMBinaryDeserialize<Metadata>(byteBlock.Buffer, 0);
            byteBlock.Dispose();

            Assert.NotNull(newMetadata);
            Assert.Equal(metadata.Count, newMetadata.Count);
            foreach (var item in metadata.Keys)
            {
                Assert.Equal(metadata[item], newMetadata[item]);
            }
        }

        [Fact]
        public void ShouldSerializeProcessInfoesBeOk()
        {
            List<ProcessInfo> infos = new List<ProcessInfo>();
            for (int i = 0; i < 200; i++)
            {
                infos.Add(new ProcessInfo() { Location = i.ToString(), Name = i.ToString(), PID = i, WinName = i.ToString() });
            }


            ByteBlock byteBlock = new ByteBlock(10240, true);
            SerializeConvert.RRQMBinarySerialize(byteBlock, infos.ToArray());
            byte[] datas = byteBlock.ToArray();
            ProcessInfo[] newinfos = SerializeConvert.RRQMBinaryDeserialize<ProcessInfo[]>(datas, 0);
            Assert.NotNull(newinfos);
            Assert.Equal(infos.Count, newinfos.Length);

            for (int i = 0; i < infos.Count; i++)
            {
                Assert.Equal(infos[i].Location, newinfos[i].Location);
                Assert.Equal(infos[i].Name, newinfos[i].Name);
                Assert.Equal(infos[i].WinName, newinfos[i].WinName);
                Assert.Equal(infos[i].PID, newinfos[i].PID);
            }
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        [InlineData(1000000)]
        public void ShouldSerializeList(int count)
        {
            List<string> list = null;

            byte[] data = SerializeConvert.RRQMBinarySerialize(list);
            var list1 = SerializeConvert.RRQMBinaryDeserialize<List<string>>(data, 0);
            Assert.Null(list1);

            list = new List<string>();
            data = SerializeConvert.RRQMBinarySerialize(list);
            var list2 = SerializeConvert.RRQMBinaryDeserialize<List<string>>(data, 0);
            Assert.NotNull(list2);
            Assert.True(list2.Count == 0);

            list = new List<string>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i.ToString());
            }
            data = SerializeConvert.RRQMBinarySerialize(list);
            var list3 = SerializeConvert.RRQMBinaryDeserialize<List<string>>(data, 0);
            Assert.NotNull(list3);
            Assert.Equal(count, list3.Count);

            for (int i = 0; i < count; i++)
            {
                Assert.Equal(list[i],list3[i]);
            }
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        [InlineData(1000000)]
        public void ShouldSerializeArray(int count)
        {
            string[] list = null;

            byte[] data = SerializeConvert.RRQMBinarySerialize(list);
            var list1 = SerializeConvert.RRQMBinaryDeserialize<string[]>(data, 0);
            Assert.Null(list1);

            list = new string[0];
            data = SerializeConvert.RRQMBinarySerialize(list);
            var list2 = SerializeConvert.RRQMBinaryDeserialize<string[]>(data, 0);
            Assert.NotNull(list2);
            Assert.True(list2.Length == 0);

            list = new string[count];
            for (int i = 0; i < count; i++)
            {
                list[i]=i.ToString();
            }
            data = SerializeConvert.RRQMBinarySerialize(list);
            var list3 = SerializeConvert.RRQMBinaryDeserialize<string[]>(data, 0);
            Assert.NotNull(list3);
            Assert.Equal(count, list3.Length);

            for (int i = 0; i < count; i++)
            {
                Assert.Equal(list[i], list3[i]);
            }
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        [InlineData(1000000)]
        public void ShouldSerializeDic(int count)
        {
            Dictionary<int,int> list = null;

            byte[] data = SerializeConvert.RRQMBinarySerialize(list);
            var list1 = SerializeConvert.RRQMBinaryDeserialize<Dictionary<int, int>>(data, 0);
            Assert.Null(list1);

            list = new  Dictionary<int, int>();
            data = SerializeConvert.RRQMBinarySerialize(list);
            var list2 = SerializeConvert.RRQMBinaryDeserialize<Dictionary<int, int>>(data, 0);
            Assert.NotNull(list2);
            Assert.True(list2.Count == 0);

            list = new  Dictionary<int, int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i,i);
            }
            data = SerializeConvert.RRQMBinarySerialize(list);
            var list3 = SerializeConvert.RRQMBinaryDeserialize<Dictionary<int, int>>(data, 0);
            Assert.NotNull(list3);
            Assert.Equal(count, list3.Count);

            for (int i = 0; i < count; i++)
            {
                Assert.Equal(list[i], list3[i]);
            }
        }
    }

    public class ProcessInfo
    {
        public string Name { get; set; }
        public int PID { get; set; }
        public string WinName { get; set; }
        public string Location { get; set; }
    }
    public class ProcessInfoShort
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
    public class Arg
    {
        public Arg()
        {
        }

        public Arg(int myProperty)
        {
            this.MyProperty = myProperty;
        }

        public int MyProperty { get; set; }
    }

    public class Student
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
        public long P3 { get; set; }
        public byte P4 { get; set; }
        public DateTime P5 { get; set; }
        public double P6 { get; set; }
        public byte[] P7 { get; set; }
        public string[] P8 { get; set; }

        public List<int> List1 { get; set; }
        public List<string> List2 { get; set; }
        public List<byte[]> List3 { get; set; }

        public Dictionary<int, int> Dic1 { get; set; }
        public Dictionary<int, string> Dic2 { get; set; }
        public Dictionary<string, string> Dic3 { get; set; }
        public Dictionary<int, Arg> Dic4 { get; set; }
    }
}