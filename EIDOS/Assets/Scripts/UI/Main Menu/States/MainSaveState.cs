using Cysharp.Threading.Tasks;
using EIDOS.Debugging;
using EIDOS.UI.Main_Menu.Transitions;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainSaveState : MainBaseState
    {
        private readonly Button slotOne;
        private readonly Button slotTwo;
        private readonly Button slotThree;
        
        public MainSaveState(
            MainMenuController menuController, 
            VisualElement elementContainer, 
            TransitionController transitionController,
            bool isInitialState = false,
            bool debug = false) 
            : base(menuController, elementContainer, transitionController, isInitialState, debug)
        {
            // Query for save slot buttons
            slotOne = elementContainer.Query<Button>("SlotOneButton");
            slotTwo = elementContainer.Query<Button>("SlotTwoButton");
            slotThree = elementContainer.Query<Button>("SlotThreeButton");
            
            // Load save data and update UI
            LoadSaveSlotData();
        }
        
        public override async UniTask Exit()
        {
            await TransitionController.TransitionOut(ElementContainer, TransitionDepth.Near);
            
            await OnExitStart();
        }

        protected override async UniTask OnEnterComplete()
        {
            // Subscribe to save slot events
            slotOne.clicked += OnSlotOneSelected;
            slotTwo.clicked += OnSlotTwoSelected;
            slotThree.clicked += OnSlotThreeSelected;
            
            await base.OnEnterComplete();
        }

        protected override async UniTask OnExitStart()
        {
            slotOne.clicked -= OnSlotOneSelected;
            slotTwo.clicked -= OnSlotTwoSelected;
            slotThree.clicked -= OnSlotThreeSelected;

            await base.OnExitStart();
        }
        
        private void LoadSaveSlotData()
        {
            // TODO: Load actual save data
            // For now, just set placeholder text
            
            slotOne.text = "Slot 1\nEmpty";
            slotTwo.text = "Slot 2\nEmpty";
            slotThree.text = "Slot 3\nEmpty";
        }

        private void OnSlotOneSelected() => SelectSlot(1);
        
        private void OnSlotTwoSelected() => SelectSlot(2);
        
        private void OnSlotThreeSelected() => SelectSlot(3);

        private void SelectSlot(int slotIndex)
        {
            Log(this, $"Selected Save Slot {slotIndex}", LogType.Info);
        }
    }
}