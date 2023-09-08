using UnityEngine;

// �������� ���� �� �ִ� Ÿ�Ե��� ���������� ������ �ϴ� �������̽�
public interface IDamageable
{
    // �������� ���� �� �ִ� Ÿ�Ե��� IDamageable�� ����ϰ� OnDamage �޼��带 �ݵ�� �����ؾ� �Ѵ�
    // OnDamage �޼���� �Է����� ������ ũ��(damage), ���� ����(hitPoint), ���� ǥ���� ����(hitNormal)�� �޴´�
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}

// �ݶ��̴� ��� �� ���� ������ ǥ�� ����
// ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
//Vector3 hitPoint = other.ClosestPoint(transform.position);
//Vector3 hitNormal = transform.position - other.transform.position;


// Ray ���� ���� ������ ǥ�� ����
// RaycastHit hit �̶��, hit.point�� hit.nomal�� ����ϸ� �ȴ�.
