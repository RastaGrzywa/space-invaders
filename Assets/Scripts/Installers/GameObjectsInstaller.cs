using UnityEngine;
using Zenject;

public class GameObjectsInstaller : MonoInstaller
{
    public GameManager gameManager;
    public PlayerController playerController;
    
    public ObjectsPooler objectsPooler;
    public ProjectileSpawner projectileSpawner;
    
    public UIController uIController;
    public GameScreen gameScreen;
    public ResultScreen resultScreen;
    
    public override void InstallBindings()
    {
        Container.BindInstance(gameManager).AsSingle().NonLazy();
        Container.BindInstance(playerController).AsSingle().NonLazy();
        Container.BindInstance(objectsPooler).AsSingle().NonLazy();
        Container.BindInstance(projectileSpawner).AsSingle().NonLazy();
        Container.BindInstance(uIController).AsSingle().NonLazy();
        Container.BindInstance(gameScreen).AsSingle().NonLazy();
        Container.BindInstance(resultScreen).AsSingle().NonLazy();
    }
}