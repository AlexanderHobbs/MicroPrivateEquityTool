
<script setup>


const props = defineProps({
    label: { type: String, required: true },
    data: {
        type: Object,
        default: () => ({})
}
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
    <div><h4>Year: {{ label }}</h4><hr/></div>
    <div><span>Revenue Amount: </span><span v-if = "data.revenue">{{ formatCurrency(data.revenue)}}</span></div>
    <div><span>Expense Amount: </span><span v-if = "data.expense">{{ formatCurrency(data.expense) }}</span></div>
    <div><span>Reported SDE Amount: </span><span v-if = "data.ReportedSDE">{{ formatCurrency(data.ReportedSDE) }}</span></div>
    <div><span>Owner Salary Amount: </span><span v-if = "data.ownerSalary">{{ formatCurrency(data.ownerSalary) }}</span></div>
  </div>

</template>


<style scoped>

.output-container {
    display: flex;
    flex-direction: column;
    gap: 22px;
    margin-bottom: 22px;
}


</style>