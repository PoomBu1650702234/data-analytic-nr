using UnityEngine;

public class RewardOnDestroy : MonoBehaviour
{
    [SerializeField] private int coinReward = 10;
    private void OnDestroy()
    {
        int tempCoin = Shop.instance.GetCoin();
        Shop.instance.SetCoin(tempCoin + coinReward);
    }
}
