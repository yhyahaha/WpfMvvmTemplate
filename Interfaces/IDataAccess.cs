using System;
using System.Collections.Generic;

namespace InterFaces
{
    public interface IDataAccess<T> : IDisposable
    {
        /// <summary>
        /// DBのすべてのデータを抽出する
        /// </summary>
        /// <returns>空の場合は空のコレクションを返す</returns>
        IList<T> GetAllItems();


        /// <summary>
        /// 指定したキーワードを含むデータを抽出する
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>空の場合は空のコレクションを返す</returns>
        IList<T> GetItemsByKeyword(string keyword);


        /// <summary>
        /// 指定したIDのデータを取得する
        /// </summary>
        /// <param name="item"></param>
        /// <returns>T型のインスタンスを返す。該当ない場合はnew Tをかえす。</returns>
        T GetItemById(int id);


        /// <summary>
        /// ItemをDBに新規登録する。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>登録時のIDを返す</returns>
        int AddItem(T item);


        /// <summary>
        /// Itemの情報を上書き保存。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>成功1,失敗0</returns>
        int UpdateItem(T item);


        /// <summary>
        /// Itemを削除する。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>成功1,失敗0</returns>
        int DeleteItem(T item);


        void Dispose();

    }
}
