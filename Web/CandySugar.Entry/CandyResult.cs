namespace CandySugar.Entry
{
    /// <summary>
    /// 结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CandyResult<T>
    {
        /// <summary>
        /// 结果
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Info { get; set; }
        internal static CandyResult<T> Result(T input,int Code=200,string Info= "操作成功")
        {
            return new CandyResult<T>
            {
                Code = Code,
                Data = input,
                Info = Info
            };
        }
    }
}
