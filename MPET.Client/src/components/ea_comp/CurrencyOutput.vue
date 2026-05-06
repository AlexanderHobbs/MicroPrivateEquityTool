
<script setup>


const props = defineProps({
    label: { type: String, required: true },
    data: { type: Object, default: null}
});

const currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
});

const formatCurrency = (value) => {
    if(value === null || value === undefined) return '-';
  return currencyFormatter.format(value);
};

</script>

<template>

  <div class = "output-container">

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Year: </span>
      <span v-if = "label">{{label}}</span><span v-else> — </span></div>

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Revenue Amount: </span>
      <span v-if = "data.revenue">{{ formatCurrency(data.revenue)}}</span><span v-else> — </span></div>

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Expense Amount: </span>
      <span v-if = "data.expense">{{ formatCurrency(data.expense) }}</span><span v-else> — </span></div>

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Reported SDE Amount: </span>
      <span v-if = "data.ReportedSDE">{{ formatCurrency(data.ReportedSDE) }}</span><span v-else> — </span></div>

    <div class = "data"><img src = "../../assets/navbar-icons/debt.png"/><span>Owner Salary Amount: </span>
      <span v-if = "data.ownerSalary">{{ formatCurrency(data.ownerSalary) }}</span><span v-else> — </span></div>
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