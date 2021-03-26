using UnityEngine;
using Zenject;

public class MaterialInstaller : MonoInstaller
{
    [SerializeField] private Materials _materials;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Materials>().FromInstance(_materials).AsSingle();
    }
}