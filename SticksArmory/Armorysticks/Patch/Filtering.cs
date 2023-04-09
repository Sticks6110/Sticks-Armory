using HarmonyLib;
using I2.Loc;
using KSP.OAB;
using System;
using System.Collections.Generic;
using System.Text;
using static KSP.OAB.AssemblyPartsPicker;

namespace SticksArmory.Patch
{
    //[HarmonyPatch(typeof(AssemblyPartsPicker), nameof(ObjectAssemblyFlexibleModal.SetVisiblePartFilters))]
    class Filtering
    {

        //SetVisiblePartFilters

        /*public static void Postfix(List<IObjectAssemblyAvailablePart> parts)
        {
            List<AssemblyFilterContainer> partFilterContainers = _partFilterContainers;
            switch (currentFilter)
            {
                case AssemblyPartFilterType.Alphanumeric:
                    {
                        Array values = Enum.GetValues(typeof(AssemblyAlphanumericFilterType));
                        int length3 = values.Length;
                        ResizeFilterCount(length3);
                        int num7 = 0;
                        int num8 = length3 - 1;
                        while (num7 < length3)
                        {
                            AssemblyFilterContainer filter3 = partFilterContainers[num7];
                            int num9 = (_isSortOrderAscending.GetValue() ? num7 : num8);
                            filter3.SetFilter(AssemblyPartFilterType.Alphanumeric, num9);
                            filter3.name = "FILTER ALPHABETICAL";
                            SetAssemblyFilterHeaderName(ref filter3, Enum.GetName(typeof(AssemblyAlphanumericFilterType), values.GetValue(num9)));
                            num7++;
                            num8--;
                        }
                        break;
                    }
                case AssemblyPartFilterType.Size:
                    {
                        int length = Enum.GetValues(typeof(AssemblySizeFilterType)).Length;
                        ResizeFilterCount(length);
                        int num = 0;
                        int num2 = length - 1;
                        while (num < length)
                        {
                            AssemblyFilterContainer filter = partFilterContainers[num];
                            int num3 = (_isSortOrderAscending.GetValue() ? num : num2);
                            filter.SetFilter(AssemblyPartFilterType.Size, num3);
                            SetAssemblyFilterHeaderName(ref filter, FilterUtils.GetSizeFullName((AssemblySizeFilterType)num3));
                            filter.SetFilterColor(GetFilterColorFromFilterEnum(num3));
                            filter.SetFilterHighlightColor(GetFilterHighlightColorFromFilterEnum(num3));
                            num++;
                            num2--;
                        }
                        break;
                    }
                case AssemblyPartFilterType.Unfiltered:
                    {
                        int count2 = 1;
                        ResizeFilterCount(count2);
                        AssemblyFilterContainer assemblyFilterContainer2 = partFilterContainers[0];
                        assemblyFilterContainer2.ShowHeader(show: false);
                        assemblyFilterContainer2.SetFilter(AssemblyPartFilterType.Unfiltered, 0);
                        assemblyFilterContainer2.name = "FILTER UNFILTERED";
                        break;
                    }
                case AssemblyPartFilterType.Mass:
                    {
                        int length2 = Enum.GetValues(typeof(AssemblyMassFilterType)).Length;
                        ResizeFilterCount(length2);
                        int num4 = 0;
                        int num5 = length2 - 1;
                        while (num4 < length2)
                        {
                            AssemblyFilterContainer filter2 = partFilterContainers[num4];
                            filter2.name = "FILTER MASS";
                            int num6 = (_isSortOrderAscending.GetValue() ? num4 : num5);
                            filter2.SetFilter(AssemblyPartFilterType.Mass, num6);
                            SetAssemblyFilterHeaderName(ref filter2, FilterUtils.GetMassFilterTypeName(num6));
                            filter2.SetFilterColor(GetFilterColorFromFilterEnum(num6));
                            filter2.SetFilterHighlightColor(GetFilterHighlightColorFromFilterEnum(num6));
                            num4++;
                            num5--;
                        }
                        break;
                    }
                case AssemblyPartFilterType.Type:
                    {
                        List<string> familiesForPartsList = GetFamiliesForPartsList(parts);
                        familiesForPartsList.Sort(CompareFilters);
                        int count = familiesForPartsList.Count;
                        ResizeFilterCount(count);
                        for (int i = 0; i < count; i++)
                        {
                            AssemblyFilterContainer assemblyFilterContainer = partFilterContainers[i];
                            assemblyFilterContainer.SetFilter(AssemblyPartFilterType.Type, i);
                            assemblyFilterContainer.Family = familiesForPartsList[i];
                            assemblyFilterContainer.name = "FILTER TYPE";
                            assemblyFilterContainer.SetFullName(LocalizationManager.GetTranslation("VAB/PartsPicker/" + familiesForPartsList[i]));
                        }
                        break;
                    }
            }
            UpdateFilterButtons();
        }*/

    }
}
