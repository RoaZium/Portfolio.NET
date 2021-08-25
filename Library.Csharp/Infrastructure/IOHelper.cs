using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PrismWPF.Common.Infrastructure
{
    public class IOHelper
    {
        #region Write
        /// <summary>
        /// 간단하게 오브젝트를 스트림에 쓰기 위한 메소드
        /// </summary>
        /// <param name="stream">쓰기 스트림</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <param name="isAsync">
        /// 비동기 여부<para/>
        /// true일 경우 Task를 반환하고, false일 경우 null을 반환함
        /// </param>
        /// <returns>Task or null</returns>
        private static object Write(Stream stream, object obj, Encoding encoding, int bufferSize, bool isAsync)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            using (StreamWriter writer =
                encoding == null ?
                    new StreamWriter(stream) :
                    (bufferSize > 0 ?
                        new StreamWriter(stream, encoding, bufferSize) :
                        new StreamWriter(stream, encoding)
                    ))
            {
                if (isAsync)
                {
                    return writer.WriteAsync(obj.ToString());
                }
                else
                {
                    writer.Write(obj);
                    return null;
                }
            }
        }

        /// <summary>
        /// 간단하게 오브젝트를 파일에 쓰기 위한 메소드
        /// </summary>
        /// <param name="filePath">쓰기 파일 경로</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <param name="isAsync">
        /// 비동기 여부<para/>
        /// true일 경우 Task를 반환하고, false일 경우 null을 반환함
        /// </param>
        /// <returns>Task or null</returns>
        private static object Write(string filePath, object obj, FileMode mode, FileAccess access, Encoding encoding, int bufferSize, bool isAsync)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            string directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);

            FileStream stream = new FileStream(filePath, mode, access);

            return Write(stream, obj, encoding, bufferSize, isAsync);
        }

        private static object Write(string filePath, object obj, FileMode mode, Encoding encoding, int bufferSize, bool isAsync)
        {
            return Write(filePath, obj, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), encoding, bufferSize, false);
        }

        /// <summary>
        /// 간단하게 오브젝트를 스트림에 쓰기 위한 메소드
        /// </summary>
        /// <param name="stream">쓰기 스트림</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <returns></returns>
        public static void Write(Stream stream, object obj, Encoding encoding = null, int bufferSize = -1)
        {
            Write(stream, obj, encoding, bufferSize, false);
        }

        /// <summary>
        /// 간단하게 오브젝트를 스트림에 쓰기 위한 메소드
        /// </summary>
        /// <param name="filePath">쓰기 파일 경로</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <returns></returns>
        public static void Write(string filePath, object obj, FileMode mode,
            FileAccess access, Encoding encoding = null, int bufferSize = -1)
        {
            Write(filePath, obj, mode, access, encoding, bufferSize, false);
        }

        /// <summary>
        /// 간단하게 오브젝트를 스트림에 쓰기 위한 메소드
        /// </summary>
        /// <param name="filePath">쓰기 파일 경로</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <returns></returns>
        public static void Write(string filePath, object obj, FileMode mode = FileMode.Create,
            Encoding encoding = null, int bufferSize = -1)
        {
            Write(filePath, obj, mode, encoding, bufferSize, false);
        }

        /// <summary>
        /// 간단하게 오브젝트를 스트림에 비동기적으로 쓰기 위한 메소드
        /// </summary>
        /// <param name="stream">쓰기 스트림</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <returns>비동기 Task</returns>
        public static Task WriteAsync(Stream stream, object obj, Encoding encoding = null, int bufferSize = -1)
        {
            return (Task)Write(stream, obj, encoding, bufferSize, true);
        }

        /// <summary>
        /// 간단하게 오브젝트를 파일에 비동기적으로 쓰기 위한 메소드
        /// </summary>
        /// <param name="filePath">쓰기 파일 경로</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <returns>비동기 Task</returns>
        public static Task WriteAsync(string filePath, object obj, FileMode mode,
            FileAccess access, Encoding encoding = null, int bufferSize = -1)
        {
            return (Task)Write(filePath, obj, mode, access, encoding, bufferSize, true);
        }

        /// <summary>
        /// 간단하게 오브젝트를 파일에 비동기적으로 쓰기 위한 메소드
        /// </summary>
        /// <param name="filePath">쓰기 파일 경로</param>
        /// <param name="obj">스트림에 쓰려는 오브젝트</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="bufferSize">얼마만큼씩 스트림에 써나갈지</param>
        /// <returns>비동기 Task</returns>
        public static Task WriteAsync(string filePath, object obj, FileMode mode = FileMode.Create,
            Encoding encoding = null, int bufferSize = -1)
        {
            return (Task)Write(filePath, obj, mode, encoding, bufferSize, true);
        }
        #endregion

        #region Read

        /// <summary>
        /// 스트림의 처음부터 끝까지 문자열을 읽어오는 메소드<para/>
        /// 용량이 큰 파일의 스트림 같은 경우 오버플로가 발생할 수 있으니 버퍼를 활용해서 읽기 작업을 수행할 것을 권장
        /// </summary>
        /// <param name="stream">내용을 읽어올 스트림</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="isAsync">
        /// 비동기 여부<para/>
        /// true일 경우 Task&lt;string&gt;를 반환하고, false일 경우 null을 반환함
        /// </param>
        /// <returns>Task&lt;string&gt; or string</returns>
        private static object Read(Stream stream, Encoding encoding, bool isAsync)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            using (StreamReader reader =
                encoding == null ?
                    new StreamReader(stream) :
                    new StreamReader(stream, encoding))
            {
                if (isAsync)
                {
                    return reader.ReadToEndAsync();
                }
                else
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 파일의 처음부터 끝까지 문자열을 읽어오는 메소드<para/>
        /// 용량이 큰 파일 같은 경우 오버플로가 발생할 수 있으니 버퍼를 활용해서 읽기 작업을 수행할 것을 권장
        /// </summary>
        /// <param name="filePath">읽어올 파일 경로</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <param name="isAsync">
        /// 비동기 여부<para/>
        /// true일 경우 Task&lt;string&gt;를 반환하고, false일 경우 null을 반환함
        /// </param>
        /// <returns>Task&lt;string&gt; or string</returns>
        private static object Read(string filePath, Encoding encoding, bool isAsync)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            using (StreamReader reader =
                encoding == null ?
                    new StreamReader(stream) :
                    new StreamReader(stream, encoding))
            {
                if (isAsync)
                {
                    return reader.ReadToEndAsync();
                }
                else
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 스트림의 처음부터 끝까지 문자열을 읽어오는 메소드<para/>
        /// 용량이 큰 파일의 스트림 같은 경우 오버플로가 발생할 수 있으니 버퍼를 활용해서 읽기 작업을 수행할 것을 권장
        /// </summary>
        /// <param name="stream">내용을 읽어올 스트림</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <returns>읽어온 문자열</returns>
        public static string Read(Stream stream, Encoding encoding = null)
        {
            return Read(stream, encoding, false).ToString();
        }

        /// <summary>
        /// 파일의 처음부터 끝까지 문자열을 읽어오는 메소드<para/>
        /// 용량이 큰 파일 같은 경우 오버플로가 발생할 수 있으니 버퍼를 활용해서 읽기 작업을 수행할 것을 권장
        /// </summary>
        /// <param name="filePath">읽어올 파일 경로</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <returns>읽어온 문자열</returns>
        public static string Read(string filePath, Encoding encoding = null)
        {
            return Read(filePath, encoding, false).ToString();
        }

        /// <summary>
        /// 스트림의 처음부터 끝까지 문자열을 비동기로 읽어오는 메소드<para/>
        /// 용량이 큰 파일의 스트림 같은 경우 오버플로가 발생할 수 있으니 버퍼를 활용해서 읽기 작업을 수행할 것을 권장
        /// </summary>
        /// <param name="stream">내용을 읽어올 스트림</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <returns>비동기 Task</returns>
        public static Task<string> ReadAsync(Stream stream, Encoding encoding = null)
        {
            return (Task<string>)Read(stream, encoding, true);
        }

        /// <summary>
        /// 파일의 처음부터 끝까지 문자열을 비동기로 읽어오는 메소드<para/>
        /// 용량이 큰 파일 같은 경우 오버플로가 발생할 수 있으니 버퍼를 활용해서 읽기 작업을 수행할 것을 권장
        /// </summary>
        /// <param name="filePath">읽어올 파일 경로</param>
        /// <param name="encoding">인코딩 방식</param>
        /// <returns>비동기 Task</returns>
        public static Task<string> ReadAsync(string filePath, Encoding encoding = null)
        {
            return (Task<string>)Read(filePath, encoding, true);
        }
        #endregion
    }
}
