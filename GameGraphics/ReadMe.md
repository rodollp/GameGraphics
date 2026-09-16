# Game Graphics Portfolio

Unity 6 / URP 환경에서 제작한 그래픽 쇼케이스입니다.

## 위치

GameGraphics/Portfolio/PF_Scene/GraphicsPortfolio 

## 시연 방법  

마우스로 쉴드를 클릭하면 쉴드가 부서진다

## 1. 그래픽 콘셉트

마법 훈련장을 콘셉트로 제작했습니다.  
훈련용 더미 주변에 에너지 실드를 배치하고 공격에 따라 실드가 손상되는 모습을 표현했습니다.  
Shader와 Particle을 이용해 공격과 피격 상태를 시각적으로 확인할 수 있도록 구성했습니다.

## 2. 3D 요소와 이펙트 배치

훈련용 더미를 중심으로 에너지 실드를 배치했습니다.  
실드를 클릭하면 Raycast의 충돌 위치에서 Hit Spark가 짧게 발생합니다.  
이를 통해 플레이어가 공격한 위치와 공격이 적중했다는 것을 확인할 수 있도록 했습니다.

## 3. Shader Graph

에너지 실드에 Shader Graph를 사용했습니다.

- Fresnel : 실드 가장자리 표현
- Emission : 실드 발광 표현
- Noise / Voronoi : 실드 균열 표현
- Shield Ratio : 실드 체력에 따른 균열 변화

실드 체력이 감소할수록 균열이 강하게 나타나며 체력이 0이 되면 실드가 사라집니다.

## 4. PBR Material

`Mat_Stone`에 URP Lit Shader를 사용했습니다.

- Smoothness : 1
- Normal Map 적용
- Normal Strength : 1

Smoothness와 Normal Map을 이용하여 돌 표면의 재질감을 표현했습니다.

## 5. Particle System

Built-in Particle System을 이용하여 Hit Spark와 Hit Flash를 제작했습니다.

Hit Spark는 Prefab으로 제작했으며 `Play On Awake`를 끄고 실드에 공격이 적중했을 때 코드에서 재생하도록 구현했습니다.

## 6. Visual Effect Graph

VFX Graph를 이용하여 매화 꽃잎이 떨어지는 환경 이펙트를 제작했습니다.

`Spawn → Initialize → Update → Output` 구조로 구성했으며, 꽃잎이 생성된 후 아래로 떨어지면서 회전하고 흔들리도록 표현했습니다.

## 7. 코드 연동

마우스로 실드를 클릭하면 다음 순서로 동작합니다.

`마우스 클릭 → Raycast → Hit Spark 재생 → Shield Damage → Shader 변화 → Shield 파괴`

한 번 공격할 때 실드에 25의 데미지를 적용합니다.  
Particle System은 기존 이펙트를 `Stop + Clear`한 뒤 다시 재생하여 불필요한 오브젝트 중복 생성을 방지했습니다.

## 8. 구현 설정

- Render Pipeline : URP
- Shader Graph : Fresnel / Emission / Noise / Voronoi
- Shader Property : Shield Ratio (`_Shield_Ratio`)
- PBR : Smoothness / Normal Map
- Particle System : Hit Spark / Hit Flash
- Particle 재생 : C# `ParticleSystem.Play()`
- VFX Graph : 매화 꽃잎 환경 이펙트
- VFX 흐름 : Spawn → Initialize → Update → Output
- Input : Unity Input System
- 공격 판정 : Raycast