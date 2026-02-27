using UnityEngine;
using YG;

public class Review : MonoBehaviour
{
    [SerializeField] private float _reviewTimer;
    private const float REVIEWCOOLDOWNMAX = 600f;

    void Update()
    {
        _reviewTimer -= Time.deltaTime;

        if (_reviewTimer <= 0)
        {
            ShowReview();
            _reviewTimer = REVIEWCOOLDOWNMAX;
        }
    }

    private void OnDestroy()
    {
        YG2.onReviewSent -= OnReviewSent;

    }

    private void ShowReview()
    {
        if(YG2.reviewCanShow)
            YG2.ReviewShow();

        YG2.onReviewSent += OnReviewSent;
    }

    private void OnReviewSent(bool isSent)
    {
        if (isSent)
        {
            GameManager.Instance.ReceiveHint(2);
        }

    }
}
