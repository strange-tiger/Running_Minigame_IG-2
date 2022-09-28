using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;

public class RankUI : MonoBehaviour
{
    private TextMeshProUGUI[] _nicknameText;
    private TextMeshProUGUI[] _scoreText;

    private int _rankNumber;

    private void Awake()
    {
        // ��ŷ ����� ���� Ÿ��Ʋ�� �����ϰ�� ����� ǥ���ϴ� ������Ʈ�� �ڽ����� ������ �Ѵ�.
        _rankNumber = transform.childCount - 1;

        _nicknameText = new TextMeshProUGUI[_rankNumber];
        _scoreText = new TextMeshProUGUI[_rankNumber];

        for (int i = _rankNumber; i > 0; --i)
        {
            _nicknameText[i - 1] = transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            _scoreText[i - 1] = transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(UpdateRank());
    }

    public IEnumerator UpdateRank()
    {
        while (true)
        {
            UpdateRanking();
            yield return new WaitForSeconds(5);
        }
    }

    private void UpdateRanking()
    {
        List<Dictionary<string, string>> ranking = MySqlSetting.GetDataByOrderLimitRowCount
            (ERankingColumns.High_Record, _rankNumber,
            ERankingColumns.ID, ERankingColumns.High_Record);

        if(ranking.Count == 0)
        {
            Debug.LogError("��ŷ �������� �Ϳ� ���� ����");
            return;
        }

        for(int i = 0; i<_rankNumber; ++i)
        {
            _nicknameText[i].text = ranking[i]["ID"];
            _scoreText[i].text = ranking[i]["High_Record"];
        }
    }
}
