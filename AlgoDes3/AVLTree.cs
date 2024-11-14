using System;
using System.Collections.Generic;
using System.IO;

namespace AlgoDes3
{
    public class AVLTree
    {
        private const string fileName = "records.txt";

        public AVLNode Root { get; private set; }

        public int iterations;
        
        private readonly List<(int, string)> records = new List<(int, string)>();

        public AVLTree()
        {
            //зчитування з файлу даних 
            using (StreamReader file = new StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] record = line.Split(' ');
                    int key = int.Parse(record[0]);
                    string data = record[1];
                    records.Add((key, data));
                }
            }

            if (records.Count != 0)
            {
                Root = new AVLNode(records[0].Item1, records[0].Item2, null);
                if (records.Count > 1)
                {
                    for (int i = 1; i < records.Count; i++)
                    {
                        AVLNode avlNode = new AVLNode(records[i].Item1, records[i].Item2, null);
                        Add(avlNode);
                    }
                }
            }

            //Перепис файлу під авл-дерево
            RewriteFileCLR(Root);
        }
        
        // пошук нода
        private AVLNode FindNode(int key)
        {
            AVLNode current = Root;
            while (current != null)
            {
                if (key == current.Key)
                    return current;
                
                current = key < current.Key ? current.Left : current.Right;
            }
            return null;
        }
        
        //пошук нода, де вставити
        private AVLNode FindNodeToInsert(int key)
        {
            AVLNode current = Root;
            while (current != null)
            {
                if (key < current.Key)
                {
                    if (current.Left is null)
                        return current;
                    current = current.Left;
                }
                else
                {
                    if (current.Right is null)
                        return current;
                    current = current.Right;
                }
            }

            return Root;
        }

        //висота дерева
        private int TreeHeight(AVLNode avlRoot)
        {
            if (avlRoot != null)
                return 1 + Math.Max(TreeHeight(avlRoot.Left), TreeHeight(avlRoot.Right));
            return 0;
        }

        //збалансованість дерева
        private int TreeBalance(AVLNode avlRoot)
        {
            return TreeHeight(avlRoot.Left) - TreeHeight(avlRoot.Right);
        }

        //CLR-обхід для пошуку запису
        public AVLNode SearchRecord(int key, AVLNode current)
        {
            iterations++;
            if (current == null)
                return null;

            if (current.Key == key)
                return current;

            return SearchRecord(key, key < current.Key ? current.Left : current.Right);
        }

        //Балансування дерева
        private void BalanceTree(AVLNode node)
        {
            while (node != null)
            {
                int balance = TreeBalance(node);

                // Ліве перевантаження
                if (balance > 1)
                {
                    if (TreeBalance(node.Left) < 0)
                        LeftRotate(node.Left); // Ліво-правий випадок
                    RightRotate(node); // Правий поворот
                }
                // Праве перевантаження
                else if (balance < -1)
                {
                    if (TreeBalance(node.Right) > 0)
                        RightRotate(node.Right); // Право-лівий випадок
                    LeftRotate(node); // Лівий поворот
                }

                node = node.Parent; // Піднімаємося до батьківського вузла
            }
        }

        //лівий поворот
        private void LeftRotate(AVLNode node)
        {
            AVLNode newRoot = node.Right;
            node.Right = newRoot.Left;

            if (newRoot.Left != null)
                newRoot.Left.Parent = node;

            newRoot.Parent = node.Parent;
            if (node.Parent == null)
                Root = newRoot;
            else if (node == node.Parent.Left)
                node.Parent.Left = newRoot;
            else
                node.Parent.Right = newRoot;

            newRoot.Left = node;
            node.Parent = newRoot;
        }

        //правий поворот
        private void RightRotate(AVLNode node)
        {
            AVLNode newRoot = node.Left;
            node.Left = newRoot.Right;

            if (newRoot.Right != null)
                newRoot.Right.Parent = node;

            newRoot.Parent = node.Parent;
            if (node.Parent == null)
                Root = newRoot;
            else if (node == node.Parent.Right)
                node.Parent.Right = newRoot;
            else
                node.Parent.Left = newRoot;

            newRoot.Right = node;
            node.Parent = newRoot;
        }

        //приватний метод для конструктора дерева
        private void Add(AVLNode avlNode)
        {
            AVLNode nodeToInsert = new AVLNode(avlNode.Key, avlNode.Data, null);
            AVLNode nodeParentToInsert = FindNodeToInsert(nodeToInsert.Key);
            if (avlNode.Key < nodeParentToInsert.Key)
                nodeParentToInsert.Left = nodeToInsert;
            else
                nodeParentToInsert.Right = nodeToInsert;
            nodeToInsert.Parent = nodeParentToInsert;

            //балансування
            BalanceTree(nodeParentToInsert);
        }

        //додавання записа
        public void AddRecord(AVLNode avlNode)
        {
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.Write(avlNode.Key.ToString() + ' ');
                file.WriteLine(avlNode.Data);
            }

            //якщо поки нема записів, то заносимо дані у корінь
            if (records.Count == 0)
            {
                Root = new AVLNode(avlNode.Key, avlNode.Data, null);
                records.Add((avlNode.Key, avlNode.Data));
                return;
            }

            records.Add((avlNode.Key, avlNode.Data));

            //якщо вже є записи
            AVLNode nodeToInsert = new AVLNode(avlNode.Key, avlNode.Data, null);
            AVLNode nodeParentToInsert = FindNodeToInsert(nodeToInsert.Key);
            if (avlNode.Key < nodeParentToInsert.Key)
                nodeParentToInsert.Left = nodeToInsert;
            else
                nodeParentToInsert.Right = nodeToInsert;
            nodeToInsert.Parent = nodeParentToInsert;

            BalanceTree(nodeParentToInsert);
            RewriteFileCLR(Root);
        }

        //редагування запису
        public bool EditRecord(int key, string newData)
        {
            AVLNode record = SearchRecord(key, Root);
            if (record != null)
            {
                record.Data = newData;
                int keyToFind = key;
                int index = records.FindIndex(rec => rec.Item1 == keyToFind);
                records[index] = (key, newData);
                RewriteFile();
                return true;
            }

            return false;
        }

        //видалення запису
        public bool DeleteRecord(int key)
        {
            AVLNode node = FindNode(key);
            if (node != null)
            {
                DeleteNode(node);
                BalanceTree(node.Parent);
                RewriteFileCLR(Root);
                return true;
            }

            return false;
        }

        //видалення нода
        private void DeleteNode(AVLNode node)
        {
            // Вузол — листок
            if (node.Left == null && node.Right == null)
            {
                if (node == Root)
                    Root = null;
                else if (node == node.Parent.Left)
                    node.Parent.Left = null;
                else
                    node.Parent.Right = null;
            }
            // Вузол має одного нащадка
            else if (node.Left == null || node.Right == null)
            {
                AVLNode child = node.Left ?? node.Right;

                if (node == Root)
                    Root = child;
                else if (node == node.Parent.Left)
                    node.Parent.Left = child;
                else
                    node.Parent.Right = child;

                child.Parent = node.Parent;
            }
            // Вузол має двох нащадків
            else
            {
                // Знаходимо мінімальний вузол у правому піддереві (наступника)
                AVLNode successor = FindMin(node.Right);

                // Копіюємо дані наступника до вузла, який потрібно видалити
                node.Key = successor.Key;
                node.Data = successor.Data;

                // Видаляємо наступника
                DeleteNode(successor);
            }
        }

        //пошук мінімального
        private AVLNode FindMin(AVLNode node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        //перепис файлу
        private void RewriteFile()
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                foreach (var record in records)
                {
                    file.Write(record.Item1.ToString() + ' ');
                    file.WriteLine(record.Item2);
                }
            }
        }

        //перепис файлу із CLR-обходом
        private void RewriteFileCLR(AVLNode root)
        {
            records.Clear();
            RewriteRecords(root);
            RewriteFile();
        }

        //CLR-обхід дерева для перепису записів
        private void RewriteRecords(AVLNode root)
        {
            if (root != null)
            {
                records.Add((root.Key, root.Data));
                RewriteRecords(root.Left);
                RewriteRecords(root.Right);
            }
        }
    }
}