using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[Message(OuterMessage.StateInfo)]
	[ProtoContract]
	public partial class StateInfo: ProtoObject
	{
		[ProtoMember(1)]
		public Unity.Mathematics.float3 Position { get; set; }

		[ProtoMember(2)]
		public Unity.Mathematics.quaternion Rotation { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

		[ProtoMember(4)]
		public UnitDesc unitDesc { get; set; }

		[ProtoMember(5)]
		public Unity.Mathematics.float2 Input { get; set; }

		[ProtoMember(6)]
		public float moveSpeed { get; set; }

	}

	[Message(OuterMessage.UnitDesc)]
	[ProtoContract]
	public partial class UnitDesc: ProtoObject
	{
		[ProtoMember(1)]
		public int ConfigID { get; set; }

	}

	[ResponseType(nameof(M2C_Snapshot))]
	[Message(OuterMessage.C2M_StateSync)]
	[ProtoContract]
	public partial class C2M_StateSync: ProtoObject, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public StateInfo MyState { get; set; }

	}

	[Message(OuterMessage.M2C_Snapshot)]
	[ProtoContract]
	public partial class M2C_Snapshot: ProtoObject, IActorLocationResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public StateInfo MyState { get; set; }

		[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(5)]
		public Dictionary<long, StateInfo> OtherUnits { get; set; }
	}

	public static partial class OuterMessage
	{
		 public const ushort StateInfo = 10201;
		 public const ushort UnitDesc = 10202;
		 public const ushort C2M_StateSync = 10203;
		 public const ushort M2C_Snapshot = 10204;
	}
}
