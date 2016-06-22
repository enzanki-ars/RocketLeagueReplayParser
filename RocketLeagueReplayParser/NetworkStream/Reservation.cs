﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLeagueReplayParser.NetworkStream
{
    public class Reservation
    {
        public UInt32 Unknown1 { get; private set; }
        public UniqueId PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public byte Unknown2 { get; private set; }


        public static Reservation Deserialize(int replayVersion, BitReader br)
        {
            var r = new Reservation();

            r.Unknown1 = br.ReadUInt32FromBits(3);

            r.PlayerId = UniqueId.Deserialize(br);

            if (r.PlayerId.Type != UniqueId.UniqueIdType.Unknown)
            {
                r.PlayerName = br.ReadString();
            }

            // TODO: See if there is any better way to determine if we need to read these bits
            // Also the 2 versions of Reservation may be better as different classes? 
            if (replayVersion < 2)
            {
                r.Unknown2 = br.ReadBitsAsBytes(2)[0];
            }
            else
            {
                r.Unknown2 = br.ReadByte();
            }
            /*
                ReservationStatus_None,
    ReservationStatus_Reserved,
    ReservationStatus_Joining,
    ReservationStatus_InGame,
    ReservationStatus_MAX
             */

            return r;
        }

        public void Serialize(int replayVersion, BitWriter bw)
        {
            bw.WriteFixedBitCount(Unknown1, 3);
            PlayerId.Serialize(bw);
            if ( PlayerId.Type != UniqueId.UniqueIdType.Unknown)
            {
                PlayerName.Serialize(bw);
            }

            if (replayVersion < 2)
            {
                bw.WriteFixedBitCount(Unknown2, 2);
            }
            else
            {
                bw.Write(Unknown2);
            }
        }

        public override string ToString()
        {
            // TODO: Since the 2 versions are Reservation arent 2 different classes, this doesnt know which version to output
            // Make separate classes, or store the version (yuck)?
            return string.Format("Unknown1: {0} ID: {1} Name: {2} Unknown2: {3}",
                Unknown1, PlayerId, PlayerName, Unknown2);
        }
    }
}
