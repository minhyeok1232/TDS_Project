# 🎮 TDS_Project - Unity 2D Top-Down Shooter
 
> Unity 기반 2D 게임 시스템 구현 과제  
> [타워 데스티니 서바이브 (TDS)] 게임 스타일의 캐릭터/배경/풀링 시스템 구성

---

## 🛠 개발 환경
- **Engine** : Unity 2022.3.42f1
- **IDE** : JetBrains Rider 2024.3

---

## 📂 주요 스크립트

### 배경 제어 (Background)
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/b70a54c4-a6b2-4a4f-b589-671dbf58660d)
- 배경이 좌측으로 반복 이동
- Hero 및 Camera는 고정된 상태

</details>

---

### Helpers 클래스
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/0357b1ab-e0a3-4145-98ed-c420f677ea16)
- 자주 쓰는 메서드 및 데이터 구조 정리
- 예: Hero, Monster 스탯 상수, 로그 래핑 등

</details>

---

### Object Pooler
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/90544f18-c8a2-4c85-8c54-7b293f278000)
- Bullet, Monster 등 반복 생성되는 오브젝트 풀 관리
- 메모리 최소화 및 성능 최적화 목적

</details>

---

### Monster
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/10095a6a-c4ac-4418-9386-1d513e4f3077)
- 캐싱 활용을 통한 성능 최적화  
- 변수, 컴포넌트 접근 최소화

![image](https://github.com/user-attachments/assets/ae46247b-d996-4fe5-9489-f94b9fa4def2)
- LayerMask를 비트 연산으로 직접 설정

![image](https://github.com/user-attachments/assets/2f9eaafa-916a-4718-8435-0200f7f204f1)
- Raycast를 통해 전방 물체 감지  
  → Collider 겹침 방지를 위한 Ray 시작 위치 조정

![image](https://github.com/user-attachments/assets/266d9410-672d-412f-91ce-4f83e0c76823)
- 몬스터 활성화 시(OnEnable)에만 초기 정보 세팅

![image](https://github.com/user-attachments/assets/120da543-43e4-4817-aacd-7d07e0c23a81)
- 인터페이스(`IDamageable`) 사용으로 유연한 확장성 확보

</details>

---

### MonsterData (ScriptableObject)
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/448236a2-15c0-41b7-8272-02a3d7376079)
- 몬스터 정보 관리 (공격력, 이동속도 등 공통값)

</details>

---

### GameManager (싱글톤)
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/ce88afb9-8b2e-429b-870d-3a1e9837c85e)
- 제네릭 `Singleton<T>` 기반 구조 사용
- 게임 상태 관리: Run, Pause, GameOver

</details>

---

### BoxColliderResize
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/49e0b60a-b993-4f3e-a792-95b6ca6a0386)
- HP가 0이 된 박스 제거 시, **다른 몬스터가 빈 공간을 통과하지 못하게 처리**

</details>

---

### IDamageable (인터페이스)
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/5723f210-fbfe-4c17-b57c-acc54230dda2)
- Box, Monster 등 다양한 객체에 데미지를 줄 수 있도록 다형성 제공

</details>

---
## Editor
### Monster Layer 구조
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/dab52e3e-b90b-4057-8266-4126031ebc2e)

- 몬스터는 **3개의 Line 중 랜덤한 위치**에 소환됨
- 각 Line마다 고유 Layer를 사용해 **서로 충돌하지 않음**
- **Prefab 소환 시 해당 라인의 Layer를 상속**

</details>

---

### Pool 관리 구조
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/92f5e240-6879-4101-8f44-3bbd5d12fc6d)
- 모든 풀 오브젝트는 **Objects 부모 오브젝트 하위에 배치**되어 관리됨

</details>

---

## Inspector Preview
<details>
  <summary>🎇 자세히 </summary>
  
![image](https://github.com/user-attachments/assets/2ab6594f-7bd7-408d-85d9-04c411b3d85c)
- 각 라인, 풀, Manager 등 직관적으로 Inspector에서 관리 가능

</details>

