using System;
using System.Collections.Generic;
using System.Text;

namespace EbiSoft.Library
{
    /// <summary>
    /// 固定サイズの配列を使ったリングバッファ
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class RingBuffer<T> : IEnumerable<T>, IList<T>
    {
        private T[] m_array;    // 格納しているデータの配列
        private int m_count;    // 格納しているデータの個数
        private int m_min;      // 格納しているデータの開始位置
        private int m_max;      // 格納しているデータの終了位置

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="length">配列の長さ</param>
        public RingBuffer(int length)
        {
            m_array = new T[length];
            Clear();
        }

        /// <summary>
        /// 指定された論理インデックスから、実インデックスを算出する
        /// </summary>
        /// <param name="index">論理インデックス(外からこのクラスを扱うときのインデックス)</param>
        /// <returns>実インデックス(m_arrayのインデックス)</returns>
        private int GetRealIndex(int index)
        {
            // 範囲外
            if ((index < 0) || (index >= Count))
            {
                return -1;
            }

            if (m_max < m_min)
            {
                int rindex = index + m_min;
                if (rindex >= m_array.Length)
                {
                    return rindex - m_array.Length;
                }
                else
                {
                    return rindex;
                }
            }
            else
            {
                return index - m_min;
            }
        }

        #region IList<T> メンバ

        /// <summary>
        /// 指定した項目のインデックスを調べます。
        /// </summary>
        /// <param name="item">検索する T</param>
        /// <returns>リストに存在する場合は item のインデックス。それ以外の場合は -1。</returns>
        public int IndexOf(T item)
        {
            // 空っぽなら検索失敗
            if (Count == 0) return -1;

            // 検索する
            for (int i = 0; i < this.Count; i++)
            {
                if (item.Equals(this[i]))
                {
                    return i;
                }
            }

            // 該当するアイテムなし
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("The method or operation is not implemented."); ;
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// 指定したインデックスにある要素を取得または設定します。
        /// </summary>
        /// <param name="index">取得または設定する要素の、0 から始まるインデックス番号。</param>
        /// <returns> 指定したインデックスにある要素。</returns>
        public T this[int index]
        {
            get
            {
                int rindex = GetRealIndex(index);
                if (rindex == -1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return m_array[rindex];
            }
            set
            {
                int rindex = GetRealIndex(index);
                if (rindex == -1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                m_array[rindex] = value;
            }
        }

        #endregion

        #region ICollection<T> メンバ

        /// <summary>
        /// アイテムを追加する
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            // インデックスを次に進める
            m_max++;
            if (m_max == m_array.Length)
            {
                m_max = 0;
                m_min = 1;
            }
            if (m_min == m_max)
            {
                m_min++;
            }
            if (m_count == 0)
            {
                m_min = m_max;
            }

            // カウントを増やす
            if (m_count < m_array.Length)
            {
                m_count++;
            }

            // セット
            m_array[m_max] = item;
        }

        /// <summary>
        /// このリストをクリアする
        /// </summary>
        public void Clear()
        {
            m_array = new T[m_array.Length];
            m_count = 0;
            m_min = -1;
            m_max = -1;
        }

        /// <summary>
        /// アイテムが存在するかどうか調べる
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            foreach (T target in this)
            {
                if (item.Equals(target))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// データの個数
        /// </summary>
        public int Count
        {
            get { return m_count; }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(T item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<T> メンバ

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable メンバ

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        /// <summary>
        /// リングバッファの内容を配列に変換する
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] returnValue = new T[this.Count];

            for (int i = 0; i < this.Count; i++)
            {
                returnValue[i] = this[i];
            }

            return returnValue;
        }

        private class Enumerator : IEnumerator<T>
        {
            RingBuffer<T> m_list;
            T m_current;
            int index;

            #region IEnumerator<T> メンバ

            public T Current
            {
                get
                {
                    return m_current;
                }
            }

            #endregion

            #region IDisposable メンバ

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator メンバ

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return m_current;
                }
            }

            public bool MoveNext()
            {
                index++;
                if (index < m_list.Count)
                {
                    m_current = m_list[index];
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                index = 0;
                m_current = m_list[index];
            }

            #endregion

            public Enumerator(RingBuffer<T> list)
            {
                m_list = list;
                Reset();
            }
        }
    }
}
