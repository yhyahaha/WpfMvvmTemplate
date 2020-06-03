using System;
using System.Windows.Input;

namespace InterFaces
{
    public interface IOperation
    {
        /// <summary>
        /// ViewModel Itemのクリア
        /// </summary>
        ICommand ClearItemCommand { get; }


        /// <summary>
        /// itemの新規登録と上書き保存
        /// </summary>
        ICommand UpdateItemCommand { get; }


        /// <summary>
        /// itemの削除
        /// </summary>
        ICommand DeleteItemCommand { get; }


        /// <summary>
        /// itemの検索
        /// </summary>
        ICommand SearchCommand { get; }


        /// <summary>
        /// 検索ｷｰﾜｰﾄﾞのクリア
        /// </summary>
        ICommand ClearKeywordCommand { get; }


        /// <summary>
        /// 検索ｷｰﾜｰﾄﾞ
        /// </summary>
        string Keyword { get; set; }


        /// <summary>
        /// ListView上で選択されたItemのインデックス
        /// </summary>
        int SelectedIndex { get; set; }


        event EventHandler RequestClose;

    }

}
