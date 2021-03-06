﻿using System;
using System.Runtime.InteropServices;

namespace nManager.Wow.Class
{
    public class DB2Struct
    {
        /*[StructLayout(LayoutKind.Sequential)]
        public struct DB2File
        {
            public uint Magic;
            public int RecordsCount;
            public int FieldsCount;
            public int RecordSize;
            public int StringTableSize;
            private uint TableHash;
            private uint Build;
            private uint Unk1;
            public int MinId;
            public int MaxId;
            private int Locale;
            private int Unk5;
            uint32_t magic;
            uint32_t record_count;
            uint32_t field_count;
            uint32_t record_size;
            uint32_t string_table_size; // if flags & 0x01 != 0, it becomes an absolute offset to the beginning of the offset_map
            uint32_t table_hash;
            uint32_t layout_hash;  // replace "build"
            uint32_t min_id;
            uint32_t max_id;
            uint32_t locale;
            uint32_t copy_table_size;
            uint16_t flags;
            uint16_t id_index; // this is the index of the field containing ID values; this is ignored if flags & 0x04 != 0
        }*/

        [StructLayout(LayoutKind.Sequential)]
        public struct WoWClientDB2
        {
            public IntPtr VTable; // pointer to vtable
            public int NumRows; // number of rows
            public int StartArrayIndex;
            public int NumRows2;
            public int MaxIndex; // maximal row index
            public int MinIndex; // minimal row index
            public uint Unk7;
            public IntPtr Data; // pointer to actual db2 file data
            public IntPtr FirstRow; // pointer to first row
            public IntPtr Rows; // pointer to rows array - not anymore?
            public IntPtr Unk11; // ptr
            public uint Unk12; // 1
            public IntPtr Unk13; // ptr
            public uint RowEntrySize; // 2 or 4
            /*public uint Unk15;
            public uint Unk16;
            public uint Unk17;
            public uint Unk18;
            public uint Unk19;
            public uint Unk20;
            public uint Unk21;
            public uint Unk22;
            public uint Unk23;
            public uint Unk24;
            public uint Unk25;
            public uint Unk26;
            public uint Unk27;
            public uint Unk28;
            public uint Unk29;
            public uint Unk30;
            public uint Unk31;
            public uint Unk32;
            public uint Unk33;
            public uint Unk34;
            public uint Unk35;
            public uint Unk36;
            public uint Unk37;
            public uint Unk38;
            public uint Unk39;
            public uint Unk40;
            public uint Unk41;
            public uint Unk42;
            public uint Unk43;
            public uint Unk44;
            public uint Unk45;
            public uint Unk46;
            public uint Unk47;
            public uint Unk48;*/
        };
    }
}