using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Gniling _gnilingPrefab;

    public override void InstallBindings()
    {
        var player = PlayerBinding();
    }

    private PlayerGnilingBrian PlayerBinding()
    {
        var gniling = Container.InstantiatePrefab(_gnilingPrefab,new Vector3(12,-5,-5),Quaternion.identity,null);
        var playerGnilingBrain = Container.InstantiateComponent<PlayerGnilingBrian>(gniling);
        playerGnilingBrain.name = "Player";
        Container.Bind<PlayerGnilingBrian>().FromInstance(playerGnilingBrain).AsSingle();
        return playerGnilingBrain;
    }
    
}
