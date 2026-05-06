<script setup>

const props = defineProps({
    label: { type: String, required: true },
    data: {type: Object, default: null}
});


</script>

<template>

   <div class = "output-container">

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Interest Rate: </span>
      <span v-if = "data.InterestRate">{{ data.InterestRate}}</span><span v-else> — </span></div>

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Taxes: </span>
      <span v-if = "data.Taxes">{{ data.Taxes }}</span><span v-else> — </span></div>

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Depreciation: </span>
      <span v-if = "data.Depreciation">{{ data.Depreciation }}</span><span v-else> — </span></div>

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Amortization: </span>
      <span v-if = "data.Amortization">{{ data.Amortization }}</span><span v-else> — </span></div>
  </div>

</template>

<style scoped>

.output-container {
    display: flex;
    flex-direction: column;
    gap: 22px;
    margin-bottom: 22px;
}


.data {
  display: flex;
  gap: 20px;
  align-items: center;
}

.data span {
  flex: 1;
}

img {
    height: 15px;
    width: auto;
    border-radius: 15px;
    background-color: #f1f2f2;
    padding: 6px;
}


</style>