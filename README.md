# HelicopterTests
Симулятор фізики гелікоптера

Прототип фізично коректної моделі польоту гелікоптера, виконаний у межах тестового завдання. Основний акцент зроблено на фізиці руху, стабільності керування та архітектурі.

Основні можливості

Кастомна фізична модель: Симуляція польоту реалізована без використання сторонніх асетів через AddForce / AddTorque. Ураховано підйомну силу, тягу, гравітацію, інерцію, опір повітря (drag), кутовий момент (torque) та затухання підйомної сили на висоті.

Архiтектура

DIP (Inversion of Control): ізоляція системи вводу за допомогою інтерфейсу IInputService.

SRP (Single Responsibility): Чіткий поділ на ядро/інтерфейси (Core), фізику (Physics) та візуальні/звукові ефекти (Visuals).

Усі фізичні та аеродинамічні параметри винесені в ScriptableObject (HelicopterConfig).

Система вводу реалізована на базі Unity New Input System.

Структура проекту

Assets/_Project/Scripts/Core — базові абстракції та інтерфейси (IInputService).

Assets/_Project/Scripts/Physics — розрахунок сил, інерції, опору повітря, руху.

Assets/_Project/Scripts/Input — реалізація Unity New Input System та зчитування дій користувача (InputController).

Assets/_Project/Scripts/Visuals — візуальне обертання гвинтів, налаштування звуку, вивід технічної інформації в UI та слідування камери.

Assets/_Project/Profiles — асети Input System та екземпляри ScriptableObject.

Керування

Pitch & Roll (нахил вперед/назад/вбік): W / A / S / D

Collective (зліт / посадка / висота): Space / Ctrl (або Shift)

Yaw (поворот навколо осі): Q / E

Вихід: Escape

Технічна інформація

Версія Unity: Unity 6000.0 (HDRP)

