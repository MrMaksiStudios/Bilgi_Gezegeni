using UnityEngine;

public class CreateOrbital : MonoBehaviour
{
    public bool isFull = false;
    public AtomGame atomGame;
    public int storedSpin = -1;
    public GameObject s1;
    public GameObject s2;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject p6;
    public GameObject d1;
    public GameObject d2;
    public GameObject d3;
    public GameObject d4;
    public GameObject d5;
    public GameObject d6;
    public GameObject d7;
    public GameObject d8;
    public GameObject d9;
    public GameObject d10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFull && collision.CompareTag("Draggable"))
        {
            Rotation rot = collision.GetComponent<Rotation>();
            if (rot != null)
            {
                storedSpin = rot.spinState;
                isFull = true;
            }
            if (s1.GetComponent<CreateOrbital>().isFull == true && s2.GetComponent<CreateOrbital>().isFull == true)
            {
                if (s1.GetComponent<CreateOrbital>().storedSpin + s2.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
                else
                {
                    atomGame.resultText.text = "Doğru!";
                    Invoke(nameof(pO), 1f);
                }
            }
            if (p1.GetComponent<CreateOrbital>().isFull == true && p2.GetComponent<CreateOrbital>().isFull == true)
            {
                if (p1.GetComponent<CreateOrbital>().storedSpin + p2.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (p3.GetComponent<CreateOrbital>().isFull == true && p4.GetComponent<CreateOrbital>().isFull == true)
            {
                if (p3.GetComponent<CreateOrbital>().storedSpin + p4.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (p5.GetComponent<CreateOrbital>().isFull == true && p6.GetComponent<CreateOrbital>().isFull == true)
            {
                if (p5.GetComponent<CreateOrbital>().storedSpin + p6.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (p5.GetComponent<CreateOrbital>().isFull == true && p6.GetComponent<CreateOrbital>().isFull == true && p3.GetComponent<CreateOrbital>().isFull == true && p4.GetComponent<CreateOrbital>().isFull == true && p1.GetComponent<CreateOrbital>().isFull == true && p2.GetComponent<CreateOrbital>().isFull == true)
            {
                atomGame.resultText.text = "Doğru!";
                Invoke(nameof(dO), 1f);
            }
            if (d1.GetComponent<CreateOrbital>().isFull == true && d2.GetComponent<CreateOrbital>().isFull == true)
            {
                if (d1.GetComponent<CreateOrbital>().storedSpin + d2.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (d3.GetComponent<CreateOrbital>().isFull == true && d4.GetComponent<CreateOrbital>().isFull == true)
            {
                if (d3.GetComponent<CreateOrbital>().storedSpin + d4.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (d5.GetComponent<CreateOrbital>().isFull == true && d6.GetComponent<CreateOrbital>().isFull == true)
            {
                if (d5.GetComponent<CreateOrbital>().storedSpin + d6.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (d7.GetComponent<CreateOrbital>().isFull == true && d8.GetComponent<CreateOrbital>().isFull == true)
            {
                if (d7.GetComponent<CreateOrbital>().storedSpin + d8.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (d9.GetComponent<CreateOrbital>().isFull == true && d10.GetComponent<CreateOrbital>().isFull == true)
            {
                if (d9.GetComponent<CreateOrbital>().storedSpin + d10.GetComponent<CreateOrbital>().storedSpin != 1)
                {
                    atomGame.LoseLifeAndResetElectron(collision.gameObject);
                }
            }
            if (d1.GetComponent<CreateOrbital>().isFull == true && d2.GetComponent<CreateOrbital>().isFull == true && d3.GetComponent<CreateOrbital>().isFull == true && d4.GetComponent<CreateOrbital>().isFull == true && d5.GetComponent<CreateOrbital>().isFull == true && d6.GetComponent<CreateOrbital>().isFull == true && d7.GetComponent<CreateOrbital>().isFull == true && d8.GetComponent<CreateOrbital>().isFull == true && d9.GetComponent<CreateOrbital>().isFull == true && d10.GetComponent<CreateOrbital>().isFull == true)
            {
                atomGame.resultText.text = "Doğru! Oyun Bitti!";
            }
        }
        else
        {
            atomGame.LoseLifeAndResetElectron(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isFull = false;
        storedSpin = -1;

    }
    public void pO()
    {
        atomGame.pOrbitalPhase();
    }
    public void dO()
    {
        atomGame.dOrbitalPhase();
    }

    public void ResetOrbital()
    {
        isFull = false;
        storedSpin = -1;
    }
}