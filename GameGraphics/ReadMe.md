# Game Graphics Portfolio

## 1. 프로젝트 개요

- Engine : Unity 6
- Render Pipeline : URP
- Language : C#
- Platform : PC
- Concept : 마법 훈련장

셰이더와 파티클, VFX를 활용하여 마법 공격과 실드의 상태 변화를 표현하는 그래픽 쇼케이스 씬입니다.

마법 훈련장의 더미를 보호하는 에너지 실드를 공격하는 상황을 구성했습니다.
플레이어가 실드를 클릭하면 타격 이펙트가 발생하고 실드의 내구도가 감소합니다.
실드의 남은 내구도에 따라 셰이더의 시각적 표현이 변화하며, 내구도가 모두 소진되면 실드가 파괴됩니다.


## 2. 주요 구현 내용

### Shield Shader

Shader Graph를 이용하여 에너지 실드의 시각 효과를 구현했습니다.

주요 사용 요소

- Fresnel
- Emission
- Noise
- Voronoi
- Shield Ratio

실드의 기본 에너지 표현에 Fresnel과 Emission을 사용했습니다.

Noise와 Voronoi를 조합하여 실드 표면의 불규칙한 균열 형태를 만들었습니다.

`Shield Ratio` 값을 C#에서 전달하여 실드의 남은 내구도에 따라 균열의 강도가 변화하도록 구현했습니다.

Shield Ratio는 다음과 같이 사용됩니다.

- 1.0 : 실드 정상 상태
- 0.75 : 약한 손상
- 0.5 : 중간 손상
- 0.25 : 강한 손상
- 0 : 실드 파괴


## 3. Shield Damage System

실드의 최대 내구도는 100이며 한 번 공격할 때 25의 데미지를 받도록 구현했습니다.

현재 실드 값을 최대 실드 값으로 나누어 `Shield Ratio`를 계산합니다.

```csharp
float shieldRatio = currentShield / maxShield;
shieldMaterial.SetFloat("_Shield_Ratio", shieldRatio);