//PRIORITY 2
public class Utils : UnityEngine.MonoBehaviour{

	private static Utils _instance = null;

	public static Utils Singleton
	{
		get { return _instance;	}
	}

	void Awake()
	{
		_instance = this;
	}

	public UnityEngine.Vector3 Vector2ToVector3(UnityEngine.Vector2 vector2)
	{
		return new UnityEngine.Vector3(vector2.x,vector2.y,0.0f);
	}

	public UnityEngine.Vector3 Vector2ToVector3(float x, float y)
	{
		return new UnityEngine.Vector3(x,y,0.0f);
	}

	public UnityEngine.Vector2 Vector3ToVector2(UnityEngine.Vector3 vector3)
	{
		return new UnityEngine.Vector2(vector3.x,vector3.y);
	}

}

public class Tags{
	public readonly static string PLAYER = "Player";
	public readonly static string BOSS = "Boss";
	public readonly static string BASSIST_PROJECTILE_ORIGIN = "BassistProjectileOrigin";
	public readonly static string LAYER_ENEMY_HIT_COLLIDER = "EnemyHitCollider";
	public readonly static string LAYER_ENEMY_ATTACK_COLLIDER = "EnemyAttackCollider";
	public readonly static string LAYER_PLAYER_HIT_COLLIDER = "PlayerHitCollider";
	public readonly static string LAYER_PLAYER_ATTACK_COLLIDER = "PlayerAttackCollider";
}

public class Constants
{
	public readonly static int PLAYER_ONE = 0;
	public readonly static int PLAYER_TWO = 1;

	public class Vector2
	{
		public readonly static UnityEngine.Vector2 one = UnityEngine.Vector2.one;
		public readonly static UnityEngine.Vector2 up = UnityEngine.Vector2.up;
		public readonly static UnityEngine.Vector2 down = UnityEngine.Vector2.down;
		public readonly static UnityEngine.Vector2 left = UnityEngine.Vector2.left;
		public readonly static UnityEngine.Vector2 right = UnityEngine.Vector2.right;
		public readonly static UnityEngine.Vector2 zero = UnityEngine.Vector2.zero;
	}

	public class Vector3
	{
		public readonly static UnityEngine.Vector3 one = UnityEngine.Vector3.one;
		public readonly static UnityEngine.Vector3 up = UnityEngine.Vector3.up;
		public readonly static UnityEngine.Vector3 down = UnityEngine.Vector3.down;
		public readonly static UnityEngine.Vector3 left = UnityEngine.Vector3.left;
		public readonly static UnityEngine.Vector3 right = UnityEngine.Vector3.right;
		public readonly static UnityEngine.Vector3 forward = UnityEngine.Vector3.forward;
		public readonly static UnityEngine.Vector3 back = UnityEngine.Vector3.back;
		public readonly static UnityEngine.Vector3 zero = UnityEngine.Vector3.zero;
	}

	public class Quaternion
	{
		public readonly static UnityEngine.Quaternion identity = UnityEngine.Quaternion.identity;
	}
}
