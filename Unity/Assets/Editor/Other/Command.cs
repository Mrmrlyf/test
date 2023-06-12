﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;

[AttributeUsage(AttributeTargets.Field)]
internal class CommandAttribute : Attribute
{
    public readonly string option;
    public readonly bool newLine;

    public CommandAttribute(string option, bool newLine = true)
    {
        this.option = option;
        this.newLine = newLine;
    }
    public CommandAttribute(bool newLine = true)
    {
        this.option = string.Empty;
        this.newLine = newLine;
    }
}

[AttributeUsage(AttributeTargets.Field)]
internal class CommandPathAttribute : Attribute
{
    public CommandPathAttribute()
    {

    }
}
/// <summary>
/// 控制台命令
/// </summary>
public class Command
{
    private static readonly int _ReadSize = 2048;

    public event Action<string> Output = Log.Info;//输出事件
    public event Action<string> Error = Log.Error;//错误事件
    private event Action _callback;//退出事件

    private bool _run;//循环控制
    private Process _process;//cmd进程
    private Encoding _outEncoding = Encoding.Default;//输出字符编码
    private Stream _outStream;//基础输出流
    private Stream _errorStream;//错误输出流

    private byte[] _tempBuffer;//临时缓冲
    private byte[] _readBuffer = new byte[_ReadSize];//读取缓存区

    private byte[] _eTempBuffer;//临时缓冲
    private byte[] _errorBuffer = new byte[_ReadSize];//错误读取缓存区

    bool _isNoWindow;
    public Command(string fileName = "cmd.exe", string argument = "", bool isNoWindow = true, Action callback = null)
    {
        _isNoWindow = isNoWindow;
        _callback = callback;
        _process = new Process();
        _process.StartInfo.Arguments = argument;
        _process.StartInfo.FileName = fileName;       
        if (isNoWindow)
        {
            _process.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
            _process.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息        
            _process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            _process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            _process.StartInfo.CreateNoWindow = true;//不显示程序窗口        
        }

        ReStart();
    }

    /// <summary>
    /// 停止使用，关闭进程和循环线程
    /// </summary>
    public void Stop()
    {
        _run = false;
        _process.Close();
    }


    /// <summary>
    /// 重新启用
    /// </summary>
    public void ReStart()
    {
        Stop();
        _process.Start();
        _run = true;
        if (_isNoWindow)
        {
            _outStream = _process.StandardOutput.BaseStream;
            _errorStream = _process.StandardError.BaseStream;
            _process.StandardInput.AutoFlush = true;
            ReadResult();
            ErrorResult();
        }          
        if (_callback != null)
        {
            _process.WaitForExit();//等待程序执行完退出进程
            Stop();
            _callback?.Invoke();
        }
    }


    //异步读取输出结果
    private void ReadResult()
    {
        if (!_run)
        {
            return;
        }
        _outStream?.BeginRead(_readBuffer, 0, _ReadSize, ReadEnd, null);
    }

    //一次异步读取结束
    private void ReadEnd(IAsyncResult ar)
    {
        int count = _outStream.EndRead(ar);

        if (count < 1)
        {
            if (_process.HasExited)
            {
                Stop();
            }
            return;
        }

        if (_tempBuffer == null)
        {
            _tempBuffer = new byte[count];
            Buffer.BlockCopy(_readBuffer, 0, _tempBuffer, 0, count);
        }
        else
        {
            byte[] buff = _tempBuffer;
            _tempBuffer = new byte[buff.Length + count];
            Buffer.BlockCopy(buff, 0, _tempBuffer, 0, buff.Length);
            Buffer.BlockCopy(_readBuffer, 0, _tempBuffer, buff.Length, count);
        }

        if (count < _ReadSize)
        {
            string str = _outEncoding.GetString(_tempBuffer);
            if (!string.IsNullOrEmpty(str)) Output?.Invoke(str);
            _tempBuffer = null;
        }

        ReadResult();
    }


    //异步读取错误输出
    private void ErrorResult()
    {
        if (!_run)
        {
            return;
        }
        _errorStream?.BeginRead(_errorBuffer, 0, _ReadSize, ErrorCallback, null);
    }

    private void ErrorCallback(IAsyncResult ar)
    {
        int count = _errorStream.EndRead(ar);

        if (count < 1)
        {
            if (_process.HasExited)
            {
                Stop();
            }
            return;
        }

        if (_eTempBuffer == null)
        {
            _eTempBuffer = new byte[count];
            Buffer.BlockCopy(_errorBuffer, 0, _eTempBuffer, 0, count);
        }
        else
        {
            byte[] buff = _eTempBuffer;
            _eTempBuffer = new byte[buff.Length + count];
            Buffer.BlockCopy(buff, 0, _eTempBuffer, 0, buff.Length);
            Buffer.BlockCopy(_errorBuffer, 0, _eTempBuffer, buff.Length, count);
        }

        if (count < _ReadSize)
        {
            string str = _outEncoding.GetString(_eTempBuffer);
            if (!string.IsNullOrEmpty(str)) Error?.Invoke(str);
            _eTempBuffer = null;
        }

        ErrorResult();
    }

    ~Command()
    {
        _run = false;
        _process?.Close();
        _process?.Dispose();
    }

}